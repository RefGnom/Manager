using System;
using System.Reflection;
using AutoFixture;
using Manager.Core.EFCore;
using Manager.Core.IntegrationTestsCore.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.Core.IntegrationTestsCore;

[TestFixture]
public abstract class IntegrationTestBase
{
    [OneTimeSetUp]
    public void OneTimeSetUp()
    {
        ConfigureTests();
        DataContext = ServiceProvider.GetService<IDataContext>()!;
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

    /// <summary>
    ///     Fixture для создания полностью заполненных моделей случайными значениями
    /// </summary>
    protected readonly Fixture Fixture = new();


    /// <summary>
    ///     Сконфигурированный ServiceProvider, по умолчанию предоставляет все реализации и тестируемой сборки и тестовой
    ///     сборки
    /// </summary>
    protected IServiceProvider ServiceProvider = null!;

    /// <summary>
    ///     Контекст для тестов. Можно работать с ним не используя тестируемый сервис.
    ///     Null если база данных не сконфигурирована
    /// </summary>
    protected IDataContext DataContext = null!;

    protected abstract Assembly TargetTestingAssembly { get; }

    private void ConfigureTests()
    {
        var integrationTestConfigurationBuilder = IntegrationTestConfigurationBuilderFactory.Create()
            .CustomizeConfigurationManager(CustomizeConfiguration)
            .CustomizeServiceCollection(CustomizeServiceCollection)
            .UseTargetTestingAssembly(TargetTestingAssembly)
            .UseAutoRegistration()
            .UseNullLogger()
            .UseNpgDataBase();
        CustomizeConfigurationBuilder(integrationTestConfigurationBuilder);

        var integrationTestConfiguration = integrationTestConfigurationBuilder.Build();
        ServiceProvider = integrationTestConfiguration.ServiceProvider;
    }

    protected virtual void CustomizeConfiguration(IConfigurationManager configurationManager) { }

    protected virtual void CustomizeServiceCollection(IServiceCollection serviceCollection) { }
    protected virtual void CustomizeConfigurationBuilder(IIntegrationTestConfigurationBuilder builder) { }
}