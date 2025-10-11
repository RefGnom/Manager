using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
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
    IIntegrationTestConfigurationBuilder WithDataBase();
    IIntegrationTestConfigurationBuilder WithLocalServer();
    IntegrationTestConfiguration Build();
}

public class IntegrationTestConfigurationBuilder(
    ITestContainerBuilder testContainerBuilder
) : IIntegrationTestConfigurationBuilder
{
    private readonly IConfigurationManager configurationManager = new ConfigurationManager();
    private readonly IServiceCollection serviceCollection = new ServiceCollection();
    private Assembly? targetAssembly;
    private bool withAutoRegistration;
    private bool withLocalServer;
    private bool withDataBase;
    private bool withNullLogger;

    public IIntegrationTestConfigurationBuilder WithTargetTestingAssembly(Assembly targetTestingAssembly)
    {
        targetAssembly = targetTestingAssembly;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithAutoRegistration()
    {
        withAutoRegistration = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithoutAutoRegistration()
    {
        withAutoRegistration = false;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithNullLogger()
    {
        withNullLogger = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithRealLogger()
    {
        withNullLogger = false;
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

    public IIntegrationTestConfigurationBuilder WithDataBase()
    {
        withDataBase = true;
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithLocalServer()
    {
        withLocalServer = true;
        return this;
    }

    public IntegrationTestConfiguration Build()
    {
        if (targetAssembly == null)
        {
            throw new ArgumentNullException($"{nameof(targetAssembly)} should be initialized before building.");
        }

        if (withDataBase)
        {
            testContainerBuilder.WithPostgres();
            var configurationDictionary = new Dictionary<string, string?>
            {
                ["DataBaseOptions:ConnectionStringTemplate"] = testContainerBuilder.ConnectionStringTemplate,
                ["DataBaseOptions:Username"] = testContainerBuilder.Username,
                ["DataBaseOptions:Password"] = testContainerBuilder.Password,
            };
            configurationManager.AddInMemoryCollection(configurationDictionary);
            serviceCollection
                .ConfigureDb()
                .AddSingleton<IDataContext, DataContext>()
                .AddSingleton<IDbContextConfigurator, NpgTestingDbContextConfigurator>(x
                    => new NpgTestingDbContextConfigurator(
                        x.GetRequiredService<IOptions<DataBaseOptions>>(),
                        x.GetRequiredService<ILogger<DbContextConfiguratorBase>>()
                    ) { EntitiesAssembly = targetAssembly }
                );
        }

        if (withLocalServer)
        {
            SolutionRootEnvironmentVariablesLoader.Load();
            configurationManager.AddEnvironmentVariables();
            testContainerBuilder.WithServer(targetAssembly, configurationManager);
        }

        if (withAutoRegistration)
        {
            serviceCollection.UseAutoRegistrationForAssembly(targetAssembly)
                .UseAutoRegistrationForAssembly(GetTestsAssembly())
                .UseAutoRegistrationForCoreCommon();
        }

        if (withNullLogger)
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

        serviceCollection.AddSingleton<IConfiguration>(configurationManager.Build());
        return new IntegrationTestConfiguration(serviceCollection.BuildServiceProvider(), testContainerBuilder.Build());
    }

    private static Assembly GetTestsAssembly()
    {
        var testAssemblyName = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar)[^5];
        var testName = $"Manager.{testAssemblyName}";
        return Assembly.Load(testName);
    }
}