using Manager.Core.AppConfiguration;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithLocalServerAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithLocalServer;
    public ConfigurationActionType[] ExcludedTypes { get; } = [];

    public void Invoke(ConfigurationActionContext context)
    {
        SolutionRootEnvironmentVariablesLoader.Load();
        context.ConfigurationManager.AddEnvironmentVariables();
        context.TestContainerBuilder.WithServer(context.TargetAssembly, context.ConfigurationManager);
    }
}