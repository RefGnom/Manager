using System;
using System.Collections.Generic;
using System.Linq;
using Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public class IntegrationTestConfigurationBuilder(
    ConfigurationActionContext buildContext
) : IIntegrationTestConfigurationBuilder
{
    private readonly ConfigurationActionCollection configurationActionCollection = [];

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

    public IIntegrationTestConfigurationBuilder CustomizeServiceCollection(
        Action<IServiceCollection, IConfiguration> customizer
    )
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

    public IIntegrationTestConfigurationBuilder WithoutDataBase()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithoutPostgresDataBaseAction());
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithLocalServer(
        Dictionary<string, string>? envVariables = null
    )
    {
        envVariables ??= new Dictionary<string, string>();
        envVariables.TryAdd("AuthenticationServiceSetting:ApiKey", "fake api key");
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithLocalServerAction(envVariables));
        return this;
    }

    public IIntegrationTestConfigurationBuilder WithDistributedCache()
    {
        configurationActionCollection.AddActionWithRemovingExcludedActionTypes(new WithRedisAction());
        return this;
    }

    public IntegrationTestConfiguration Build()
    {
        foreach (var configurationAction in configurationActionCollection.OrderBy(x => x.Type))
        {
            configurationAction.Invoke(buildContext);
        }

        buildContext.ServiceCollection.AddSingleton<IConfiguration>(buildContext.ConfigurationManager.Build());
        return new IntegrationTestConfiguration(
            buildContext.ServiceCollection.BuildServiceProvider(),
            buildContext.TestContainerBuilder.Build()
        );
    }
}