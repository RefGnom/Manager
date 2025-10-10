using System;
using System.Collections.Generic;
using System.Reflection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Containers;
using DotNet.Testcontainers.Networks;
using Manager.Core.AppConfiguration;
using Microsoft.Extensions.Configuration;
using Testcontainers.PostgreSql;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public interface ITestContainerBuilder
{
    string ConnectionStringTemplate { get; }
    ITestContainerBuilder WithServer(Assembly assembly, IConfiguration configuration);
    ITestContainerBuilder WithPostgres(out string username, out string password);
    ContainerConfiguration Build();
}

public class TestContainerBuilder : ITestContainerBuilder
{
    private const string EnvironmentName = "Testing";

    private const int ContainerPort = 8080;

    private const int PostgresContainerPort = 5432;
    private const int PostgresHostPort = 5000;
    private const string DataBaseName = "testdb";

    private readonly INetwork network = new NetworkBuilder().Build();
    private readonly List<IContainer> containers = [];

    public string ConnectionStringTemplate { get; } =
        $"Host=127.0.0.1;Port={PostgresHostPort};Database={DataBaseName};Username={{0}};Password={{1}}";

    public ITestContainerBuilder WithServer(Assembly assembly, IConfiguration configuration)
    {
        var serverPropertiesAttribute = assembly.GetCustomAttribute<ServerPropertiesAttribute>();
        if (serverPropertiesAttribute == null)
        {
            throw new Exception($"У сервера должен быть атрибут {nameof(ServerPropertiesAttribute)}");
        }

        var port = configuration.GetValue<int>(serverPropertiesAttribute.PortKey);
        var secrets = TargetAssemblySecretsLoader.LoadAsDictionary(assembly);
        containers.Add(
            new ContainerBuilder()
                .WithImage($"{serverPropertiesAttribute.DockerContainerName}:latest")
                .WithPortBinding(port, ContainerPort)
                .WithWaitStrategy(
                    Wait.ForUnixContainer().UntilHttpRequestIsSucceeded(r => r.ForPort(ContainerPort).ForPath("health"))
                )
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", EnvironmentName)
                .WithEnvironment(secrets)
                .WithNetwork(network)
                .Build()
        );

        return this;
    }

    public ITestContainerBuilder WithPostgres(
        out string username,
        out string password
    )
    {
        username = Guid.NewGuid().ToString();
        password = Guid.NewGuid().ToString();
        var postgreSqlContainer = new PostgreSqlBuilder()
            .WithImage("postgres:16")
            .WithPortBinding(PostgresHostPort, PostgresContainerPort)
            .WithDatabase(DataBaseName)
            .WithUsername(username)
            .WithPassword(password)
            //.WithNetwork(network)
            .Build();
        containers.Add(
            postgreSqlContainer
        );

        return this;
    }

    public ContainerConfiguration Build()
    {
        return new ContainerConfiguration(network, containers.ToArray());
    }
}