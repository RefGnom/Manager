using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Manager.Core.AppConfiguration;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Manager.Core.IntegrationTestsCore.Configuration.Containers;

public class TestContainerBuilder : ITestContainerBuilder
{
    private const string EnvironmentName = "Testing";

    private const int ContainerPort = 8080;

    private const int PostgresContainerPort = 5432;
    private const int PostgresHostPort = 5000;
    private const string DataBaseName = "testdb";
    private const string NetworkAliases = "postgres";
    private readonly List<ContainerWithType> containers = [];

    private readonly INetwork network = new NetworkBuilder().Build();

    private string ContainerConnectionStringTemplate { get; } =
        $"Host={NetworkAliases};Port={PostgresContainerPort};Database={DataBaseName};Username={{0}};Password={{1}}";

    public string ConnectionStringTemplate { get; } =
        $"Host=127.0.0.1;Port={PostgresHostPort};Database={DataBaseName};Username={{0}};Password={{1}}";

    public string Username { get; } = Guid.NewGuid().ToString();
    public string Password { get; } = Guid.NewGuid().ToString();

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
                    .WithEnvironment("ASPNETCORE_ENVIRONMENT", EnvironmentName)
                    .WithEnvironment("DataBaseOptions:ConnectionStringTemplate", ContainerConnectionStringTemplate)
                    .WithEnvironment("DataBaseOptions:Username", Username)
                    .WithEnvironment("DataBaseOptions:Password", Password)
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
                    .WithPortBinding(PostgresHostPort, PostgresContainerPort)
                    .WithDatabase(DataBaseName)
                    .WithUsername(Username)
                    .WithPassword(Password)
                    .WithNetwork(network)
                    .WithNetworkAliases(NetworkAliases)
                    .Build(),
                ContainerType.DataBase
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