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
            ["RedisOptions:Host"] = context.TestContainerBuilder.RedisHost,
            ["RedisOptions:Password"] = context.TestContainerBuilder.RedisPassword,
            ["RedisOptions:Port"] = context.TestContainerBuilder.RedisHostPort.ToString(),
            ["RedisOptions:TimeoutInMs"] = context.TestContainerBuilder.RedisTimeoutInMs.ToString(),
        };
        context.ConfigurationManager.AddInMemoryCollection(configurationDictionary);
    }
}