using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithRedisAction: IConfigurationAction
{
    public ConfigurationActionType Type => ConfigurationActionType.WithDistributedCache;
    public ConfigurationActionType[] ExcludedTypes => [];

    public void Invoke(ConfigurationActionContext context)
    {
        context.TestContainerBuilder.WithRedis();
        var configurationDictionary = new Dictionary<string, string?>
        {
            ["RedisOptions:ConnectionStringTemplate"] = context.TestContainerBuilder.RedisConnectionStringTemplate,
            ["RedisOptions:Password"] = context.TestContainerBuilder.RedisPassword,
        };
        context.ConfigurationManager.AddInMemoryCollection(configurationDictionary);
    }
}