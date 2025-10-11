using System.Reflection;
using Manager.Core.Common.DependencyInjection.Attributes;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.IntegrationTestsCore.Configuration.Containers;

[IgnoreAutoRegistration]
public interface ITestContainerBuilder
{
    string ConnectionStringTemplate { get; }
    string Username { get; }
    string Password { get; }

    ITestContainerBuilder WithServer(Assembly assembly, IConfiguration configuration);
    ITestContainerBuilder WithPostgres();
    ContainerConfiguration Build();
}