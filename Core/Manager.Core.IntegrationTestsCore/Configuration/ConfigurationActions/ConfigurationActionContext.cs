using System.Reflection;
using Manager.Core.IntegrationTestsCore.Configuration.Containers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public record ConfigurationActionContext(
    IServiceCollection ServiceCollection,
    IConfigurationManager ConfigurationManager,
    ITestContainerBuilder TestContainerBuilder,
    Assembly TargetAssembly
);