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
    /// <summary>
    /// Fixture для создания полностью заполненных моделей случайными значениями
    /// </summary>
    protected readonly Fixture Fixture = new();

    protected IServiceProvider ServiceProvider = null!;

    /// <summary>
    /// Контекст для тестов. Можно работать с ним не используя тестируемый сервис
    /// </summary>
    protected IDataContext DataContext = null!;

    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        ConfigureTests();
        DataContext = ServiceProvider.GetRequiredService<IDataContext>();
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
        foreach (var entity in dataContextForTests.Entities)
        {
            try
            {
                var dbContext = dbContextWrapperFactory.Create();
                dbContext.Remove(entity);
                dbContext.SaveChanges();
            }
            catch
            {
                // ignored
            }
        }

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