using System;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class CustomizeServiceCollectionAction(
    Action<IServiceCollection> customize
) : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.CustomizeServiceCollection;
    public ConfigurationActionType[] ExcludedTypes { get; } = [];

    public void Invoke(ConfigurationActionContext context)
    {
        customize(context.ServiceCollection);
    }
}