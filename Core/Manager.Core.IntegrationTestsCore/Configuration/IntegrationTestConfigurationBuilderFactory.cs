using System.Reflection;
using Manager.Core.IntegrationTestsCore.Configuration.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration;

public static class IntegrationTestConfigurationBuilderFactory
{
    public static IIntegrationTestConfigurationBuilder Create(Assembly targetAssembly) =>
        new IntegrationTestConfigurationBuilder(
            new ServiceCollection(),
            new ConfigurationManager(),
            new TestContainerBuilder(),
            targetAssembly
        );
}