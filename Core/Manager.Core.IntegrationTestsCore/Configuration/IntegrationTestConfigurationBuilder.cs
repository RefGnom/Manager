using System;
using System.Linq;
using System.Reflection;
using Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;
using Manager.Core.IntegrationTestsCore.Configuration.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public class IntegrationTestConfigurationBuilder(
    IServiceCollection serviceCollection,
    IConfigurationManager configurationManager,
    ITestContainerBuilder testContainerBuilder,
    Assembly targetAssembly
) : IIntegrationTestConfigurationBuilder
{
    private readonly ConfigurationActionCollection configurationActionCollection = [];

    private readonly ConfigurationActionContext buildContext = new(
        serviceCollection,
        configurationManager,
        testContainerBuilder,
        targetAssembly
    );

    public IIntegrationTestConfigurationBuilder WithAutoRegistration()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithAutoRegistrationAction());
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithoutAutoRegistration()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithoutAutoRegistrationAction());
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithNullLogger()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithNullLoggerAction());
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithRealLogger()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithRealLoggerAction());
        return this;
    }

    public IIntegrationTestConfigurationBuilder CustomizeServiceCollection(Action<IServiceCollection> customizer)
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(
            new CustomizeServiceCollectionAction(customizer)
        );
        return this;
    }

    public IIntegrationTestConfigurationBuilder CustomizeConfigurationManager(Action<IConfigurationManager> customizer)
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(
            new CustomizeConfigurationAction(customizer)
        );
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithDataBase()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithPostgresDataBaseAction());
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithLocalServer()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithLocalServerAction());
        return this;
    }

    public IntegrationTestConfiguration Build()
    {
        foreach (var configurationAction in configurationActionCollection.OrderBy(x => x.Type))
        {
            configurationAction.Invoke(buildContext);
        }

        serviceCollection.AddSingleton<IConfiguration>(configurationManager.Build());
        return new IntegrationTestConfiguration(serviceCollection.BuildServiceProvider(), testContainerBuilder.Build());
    }
}