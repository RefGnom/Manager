using System;
using System.IO;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using Manager.Core.Common.DependencyInjection.Attributes;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore;
using Manager.Core.EFCore.Configuration;
using Manager.Core.Logging.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Manager.Core.IntegrationTestsCore.Configuration;

[IgnoreAutoRegistration]
public interface IIntegrationTestConfigurationBuilder
{
    IIntegrationTestConfigurationBuilder UseTargetTestingAssembly(Assembly targetTestingAssembly);
    IIntegrationTestConfigurationBuilder UseAutoRegistration();
    IIntegrationTestConfigurationBuilder NotUseAutoRegistration();
    IIntegrationTestConfigurationBuilder UseNullLogger();
    IIntegrationTestConfigurationBuilder UseRealLogger();
    IIntegrationTestConfigurationBuilder CustomizeServiceCollection(Action<IServiceCollection> customizer);
    IIntegrationTestConfigurationBuilder CustomizeConfigurationManager(Action<IConfigurationManager> customizer);
    IIntegrationTestConfigurationBuilder UseNpgDataBase();
    IIntegrationTestConfigurationBuilder UseLocalServer();
    IntegrationTestConfiguration Build();
}

public class IntegrationTestConfigurationBuilder : IIntegrationTestConfigurationBuilder
{
    private const int ContainerPort = 8080;

    private readonly IConfigurationManager configurationManager = new ConfigurationManager();
    private readonly IServiceCollection serviceCollection = new ServiceCollection();
    private Assembly? targetAssembly;
    private bool useAutoRegistration;
    private bool useLocalServer;
    private bool useNpgDataBase;
    private bool useNullLogger;

    public IIntegrationTestConfigurationBuilder UseTargetTestingAssembly(Assembly targetTestingAssembly)
    {
        targetAssembly = targetTestingAssembly;
        return this;
    }

    public IIntegrationTestConfigurationBuilder UseAutoRegistration()
    {
        useAutoRegistration = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder NotUseAutoRegistration()
    {
        useAutoRegistration = false;
        return this;
    }

    public IIntegrationTestConfigurationBuilder UseNullLogger()
    {
        useNullLogger = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder UseRealLogger()
    {
        useNullLogger = false;
        return this;
    }

    public IIntegrationTestConfigurationBuilder CustomizeServiceCollection(Action<IServiceCollection> customizer)
    {
        customizer(serviceCollection);
        return this;
    }

    public IIntegrationTestConfigurationBuilder CustomizeConfigurationManager(Action<IConfigurationManager> customizer)
    {
        customizer(configurationManager);
        return this;
    }

    public IIntegrationTestConfigurationBuilder UseNpgDataBase()
    {
        useNpgDataBase = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder UseLocalServer()
    {
        useLocalServer = true;
        return this;
    }

    public IntegrationTestConfiguration Build()
    {
        if (targetAssembly == null)
        {
            throw new ArgumentNullException($"{nameof(targetAssembly)} should be initialized before building.");
        }

        serviceCollection.AddSingleton<IConfiguration>(configurationManager.Build());

        if (useAutoRegistration)
        {
            serviceCollection.UseAutoRegistrationForAssembly(targetAssembly)
                .UseAutoRegistrationForAssembly(GetTestsAssembly())
                .UseAutoRegistrationForCoreCommon();
        }

        if (useNullLogger)
        {
            serviceCollection
                .AddSingleton<ILogger, NullLogger>()
                .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
        }
        else
        {
            serviceCollection
                .AddLogging(x => x.AddConsole())
                .AddCustomLogger(configurationManager, Environments.Development);
        }

        if (useNpgDataBase)
        {
            serviceCollection
                .ConfigureDb()
                .AddSingleton<DataContext, DataContext>()
                .AddSingleton<IDataContext, DataContextForTests>()
                .AddSingleton<IDbContextConfigurator, NpgTestingDbContextConfigurator>(x
                    => new NpgTestingDbContextConfigurator(
                        x.GetRequiredService<IOptions<DataBaseOptions>>(),
                        x.GetRequiredService<ILogger<DbContextConfiguratorBase>>()
                    ) { EntitiesAssembly = targetAssembly }
                );
        }

        var container = useLocalServer
            ? new ContainerBuilder()
                .WithImage("manager-authentication-service:latest")
                .WithPortBinding(8081, ContainerPort)
                .WithWaitStrategy(
                    Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(ContainerPort).ForPath("health"))
                )
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("DataBaseOptions:Username", "")
                .WithEnvironment("DataBaseOptions:Password", "")
                .Build()
            : null;

        return new IntegrationTestConfiguration(serviceCollection.BuildServiceProvider(), container);
    }

    private static Assembly GetTestsAssembly()
    {
        var testAssemblyName = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar)[^5];
        var testName = $"Manager.{testAssemblyName}";
        return Assembly.Load(testName);
    }
}