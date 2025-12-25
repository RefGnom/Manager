using System.Collections.Generic;
using System.Reflection;
using Manager.Core.Common.DependencyInjection.Attributes;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.IntegrationTestsCore.Configuration.Containers;

[IgnoreAutoRegistration]
public interface ITestContainerBuilder
{
    string ConnectionStringTemplate { get; }
    string PostgresUsername { get; }
    string PostgresPassword { get; }
    string RedisHost { get; }
    int RedisHostPort { get; }
    string RedisPassword { get; }

    void WithServer(Assembly assembly, IConfiguration configuration, IReadOnlyDictionary<string, string> envVariables);
    void WithPostgres();
    void WithRedis();
    ContainerConfiguration Build();
}