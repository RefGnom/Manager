using System;
using System.IO;
using System.Reflection;
using AutoFixture;
using Manager.Core.AppConfiguration.DataBase;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Manager.Core.IntegrationTestsCore;

[TestFixture]
public abstract class IntegrationTestBase
{
    protected abstract Assembly TargetTestingAssembly { get; }
    protected virtual bool UseNullLogger => true;

    protected readonly Fixture Fixture = new();
    protected IServiceProvider ServiceProvider = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        var configuration = new ConfigurationManager();
        CustomizeConfiguration(configuration);
        ServiceProvider = new ServiceCollection()
            .AddSingleton<IConfiguration>(configuration)
            .AddLogging(configuration, UseNullLogger)
            .UseAutoRegistrationForAssembly(TargetTestingAssembly)
            .UseAutoRegistrationForAssembly(GetTestsAssembly())
            .UseAutoRegistrationForCoreCommon()
            .ConfigureDb()
            .AddSingleton<IDbContextConfigurator, NpgTestingDbContextConfigurator>(x
                => new NpgTestingDbContextConfigurator(
                    x.GetRequiredService<IOptions<DataBaseOptions>>(),
                    x.GetRequiredService<ILogger<DbContextConfiguratorBase>>()
                ) { EntitiesAssembly = TargetTestingAssembly }
            )
            .BuildServiceProvider();
    }

    protected abstract void CustomizeConfiguration(IConfigurationManager configurationManager);

    private static Assembly GetTestsAssembly()
    {
        var testAssemblyName = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar)[^5];
        var testName = $"Manager.{testAssemblyName}";
        return Assembly.Load(testName);
    }
}