using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class CustomizeServiceCollectionAction(
    Action<IServiceCollection, IConfiguration> customize
) : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.CustomizeServiceCollection;
    public ConfigurationActionType[] ExcludedTypes { get; } = [];

    public void Invoke(ConfigurationActionContext context)
    {
        customize(context.ServiceCollection, context.ConfigurationManager.Build());
    }
}