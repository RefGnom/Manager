using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Networks;
using Manager.Core.AppConfiguration;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public class TestContainerBuilder : ITestContainerBuilder
{
    private const string EnvironmentName = "Testing";

    private const int ContainerPort = 8080;

    private const int PostgresContainerPort = 5432;
    private const int PostgresHostPort = 5000;
    private const string DataBaseName = "testdb";
    private const string NetworkAliases = "postgres";

    private readonly INetwork network = new NetworkBuilder().Build();
    private readonly List<ContainerWithType> containers = [];

    public string ConnectionStringTemplate { get; } =
        $"Host=127.0.0.1;Port={PostgresHostPort};Database={DataBaseName};Username={{0}};Password={{1}}";

    private string ContainerConnectionStringTemplate { get; } =
        $"Host={NetworkAliases};Port={PostgresContainerPort};Database={DataBaseName};Username={{0}};Password={{1}}";

    public string Username { get; } = Guid.NewGuid().ToString();
    public string Password { get; } = Guid.NewGuid().ToString();

    public ITestContainerBuilder WithServer(
        Assembly assembly,
        IConfiguration configuration
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
                    .WithNetwork(network)
                    .Build(),
                ContainerType.Server
            )
        );

        return this;
    }

    public ITestContainerBuilder WithPostgres()
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

        return this;
    }

    public ContainerConfiguration Build()
    {
        return new ContainerConfiguration(
            network,
            containers.OrderBy(x => x.Type).ToArray()
        );
    }
}