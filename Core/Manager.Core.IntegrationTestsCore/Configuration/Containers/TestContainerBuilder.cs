using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Manager.Core.AppConfiguration;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;
using Testcontainers.Redis;

namespace Manager.Core.IntegrationTestsCore.Configuration.Containers;

public class TestContainerBuilder : ITestContainerBuilder
{
    private const string EnvironmentName = "Testing";

    private const int ContainerPort = 8080;

    private const int RedisContainerPort = 6379;
    private const int PostgresContainerPort = 5432;
    private const string DataBaseName = "testdb";
    private const string PostgresNetworkAliases = "postgres";
    private const string RedisNetworkAliases = "redis";
    private readonly List<ContainerWithType> containers = [];

    private readonly INetwork network = new NetworkBuilder().Build();
    private readonly int postgresHostPort = Random.Shared.Next(9_000, 10_000);

    private string ContainerConnectionStringTemplate { get; } =
        $"Host={PostgresNetworkAliases};Port={PostgresContainerPort};Database={DataBaseName};Username={{0}};Password={{1}}";

    public string ConnectionStringTemplate =>
        $"Host=127.0.0.1;Port={postgresHostPort};Database={DataBaseName};Username={{0}};Password={{1}}";

    public string RedisHost => RedisNetworkAliases;
    public int RedisHostPort { get; } = Random.Shared.Next(8_000, 9_000);

    public string PostgresUsername { get; } = Guid.NewGuid().ToString();
    public string PostgresPassword { get; } = Guid.NewGuid().ToString();
    public string RedisPassword { get; } = Guid.NewGuid().ToString();

    public void WithServer(
        Assembly assembly,
        IConfiguration configuration,
        IReadOnlyDictionary<string, string> envVariables
    )
    {
        var serverPropertiesAttribute = assembly.GetCustomAttribute<ServerPropertiesAttribute>();
        if (serverPropertiesAttribute == null)
        {
            throw new Exception($"У сервера должен быть атрибут {nameof(ServerPropertiesAttribute)}");
        }

        var port = configuration.GetValue<int>(serverPropertiesAttribute.PortKey);
        containers.Add(
            new ContainerWithType(
                new ContainerBuilder()
                    .WithImage($"{serverPropertiesAttribute.DockerContainerName}:latest")
                    .WithPortBinding(port, ContainerPort)
                    .WithWaitStrategy(
                        Wait.ForUnixContainer()
                            .UntilHttpRequestIsSucceeded(r => r.ForPort(ContainerPort).ForPath("health"))
                    )
                    .WithOutputConsumer(Consume.RedirectStdoutAndStderrToConsole())
                    .WithEnvironment("ASPNETCORE_ENVIRONMENT", EnvironmentName)
                    .WithEnvironment("DataBaseOptions:ConnectionStringTemplate", ContainerConnectionStringTemplate)
                    .WithEnvironment("DataBaseOptions:Username", PostgresUsername)
                    .WithEnvironment("DataBaseOptions:Password", PostgresPassword)
                    .WithEnvironment("RedisOptions:Host", $"{RedisNetworkAliases}")
                    .WithEnvironment("RedisOptions:Port", RedisContainerPort.ToString())
                    .WithEnvironment("RedisOptions:Password", RedisPassword)
                    .WithEnvironment("RedisOptions:TimeoutInMs", "5000")
                    .WithEnvironment(envVariables)
                    .WithNetwork(network)
                    .Build(),
                ContainerType.Server
            )
        );
    }

    public void WithPostgres()
    {
        containers.Add(
            new ContainerWithType(
                new PostgreSqlBuilder()
                    .WithImage("postgres:16")
                    .WithPortBinding(postgresHostPort, PostgresContainerPort)
                    .WithDatabase(DataBaseName)
                    .WithUsername(PostgresUsername)
                    .WithPassword(PostgresPassword)
                    .WithNetwork(network)
                    .WithNetworkAliases(PostgresNetworkAliases)
                    .Build(),
                ContainerType.DataBase
            )
        );
    }

    public void WithRedis()
    {
        containers.Add(
            new ContainerWithType(
                new RedisBuilder()
                    .WithImage("redis:7-alpine")
                    .WithPortBinding(RedisHostPort, RedisContainerPort)
                    .WithCommand("redis-server", "--requirepass", RedisPassword)
                    .WithNetwork(network)
                    .WithNetworkAliases(RedisNetworkAliases)
                    .WithWaitStrategy(
                        Wait.ForUnixContainer()
                            .UntilInternalTcpPortIsAvailable(RedisContainerPort)
                    )
                    .Build(),
                ContainerType.Cache
            )
        );
    }

    public ContainerConfiguration Build()
    {
        return new ContainerConfiguration(
            network,
            containers.OrderBy(x => x.Type).ToArray()
        );
    }
}