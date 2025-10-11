using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public interface ITestContainerBuilder
{
    string ConnectionStringTemplate { get; }
    string Username { get; }
    string Password { get; }

    ITestContainerBuilder WithServer(Assembly assembly, IConfiguration configuration);
    ITestContainerBuilder WithPostgres();
    ContainerConfiguration Build();
}