using System;
using System.IO;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using Manager.Core.AppConfiguration;
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
    IIntegrationTestConfigurationBuilder WithTargetTestingAssembly(Assembly targetTestingAssembly);
    IIntegrationTestConfigurationBuilder WithAutoRegistration();
    IIntegrationTestConfigurationBuilder WithoutAutoRegistration();
    IIntegrationTestConfigurationBuilder WithNullLogger();
    IIntegrationTestConfigurationBuilder WithRealLogger();
    IIntegrationTestConfigurationBuilder CustomizeServiceCollection(Action<IServiceCollection> customizer);
    IIntegrationTestConfigurationBuilder CustomizeConfigurationManager(Action<IConfigurationManager> customizer);
    IIntegrationTestConfigurationBuilder WithNpgDataBase();
    IIntegrationTestConfigurationBuilder WithLocalServer();
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

    public IIntegrationTestConfigurationBuilder WithTargetTestingAssembly(Assembly targetTestingAssembly)
    {
        targetAssembly = targetTestingAssembly;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithAutoRegistration()
    {
        useAutoRegistration = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithoutAutoRegistration()
    {
        useAutoRegistration = false;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithNullLogger()
    {
        useNullLogger = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithRealLogger()
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

    public IIntegrationTestConfigurationBuilder WithNpgDataBase()
    {
        useNpgDataBase = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithLocalServer()
    {
        useLocalServer = true;
        return this;
    }

    public IntegrationTestConfiguration Build()
    {
        if (useLocalServer)
        {
            SolutionRootEnvironmentVariablesLoader.Load();
            configurationManager.AddEnvironmentVariables();
        }

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

        if (!useLocalServer)
        {
            return new IntegrationTestConfiguration(serviceCollection.BuildServiceProvider(), null);
        }

        var serverPropertiesAttribute = targetAssembly.GetCustomAttribute<ServerPropertiesAttribute>();
        if (serverPropertiesAttribute == null)
        {
            throw new Exception($"У тестируемого сервера должен быть атрибут {nameof(ServerPropertiesAttribute)}");
        }

        var port = configurationManager.GetValue<int>(serverPropertiesAttribute.PortKey);
        var secrets = TargetAssemblySecretsLoader.LoadAsDictionary(targetAssembly);
        var container = new ContainerBuilder()
            .WithImage($"{serverPropertiesAttribute.DockerContainerName}:latest")
            .WithPortBinding(port, ContainerPort)
            .WithWaitStrategy(
                Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(ContainerPort).ForPath("health"))
            )
            .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Testing")
            .WithEnvironment(secrets)
            .Build();

        return new IntegrationTestConfiguration(serviceCollection.BuildServiceProvider(), container);
    }

    private static Assembly GetTestsAssembly()
    {
        var testAssemblyName = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar)[^5];
        var testName = $"Manager.{testAssemblyName}";
        return Assembly.Load(testName);
    }
}