using System;
using System.Reflection;
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        ConfigureTests();
    }

    private void ConfigureTests()
    {
        var configuration = new ConfigurationManager();
        CustomizeConfiguration(configuration);

        var serviceCollection = new ServiceCollection()
            .ConfigureForIntegrationTests(configuration, TargetTestingAssembly, UseNullLogger);
        CustomizeServiceCollection(serviceCollection);

        ServiceProvider = serviceCollection.BuildServiceProvider();
    }

    protected abstract void CustomizeConfiguration(IConfigurationManager configurationManager);

    protected virtual IServiceCollection CustomizeServiceCollection(IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}