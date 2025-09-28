using System;
using System.Reflection;
using AutoFixture;
using Manager.Core.EFCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.Core.IntegrationTestsCore;

[TestFixture]
public abstract class IntegrationTestBase
{
    protected readonly Fixture Fixture = new();
    protected IServiceProvider ServiceProvider = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        ConfigureTests();
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        var dataContext = ServiceProvider.GetRequiredService<IDataContext>();
        if (dataContext is not DataContextForTests dataContextForTests)
        {
            return;
        }

        var dbContextWrapperFactory = ServiceProvider.GetRequiredService<IDbContextWrapperFactory>();
        var dbContext = dbContextWrapperFactory.Create();
        dbContext.RemoveRange(dataContextForTests.Entities);
        dbContext.SaveChanges();
        dataContextForTests.Entities.Clear();
    }

    #region Configuration

    protected abstract Assembly TargetTestingAssembly { get; }
    protected virtual bool UseNullLogger => true;

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

    #endregion
}