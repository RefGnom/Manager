using System;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class CustomizeConfigurationAction(
    Action<IConfigurationManager> customize
) : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.CustomizeConfiguration;
    public ConfigurationActionType[] ExcludedTypes { get; } = [];

    public void Invoke(ConfigurationActionContext context)
    {
        customize(context.ConfigurationManager);
    }
}