using System.Collections.Generic;
using Manager.Core.AppConfiguration;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithLocalServerAction(
    IReadOnlyDictionary<string, string> envVariables
) : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithLocalServer;
    public ConfigurationActionType[] ExcludedTypes { get; } = [];

    public void Invoke(ConfigurationActionContext context)
    {
        var loadedVariables = SolutionRootEnvironmentVariablesLoader.Load();
        foreach (var variable in envVariables)
        {
            loadedVariables[variable.Key] = variable.Value;
        }

        context.ConfigurationManager.AddEnvironmentVariables();
        context.TestContainerBuilder.WithServer(context.TargetAssembly, context.ConfigurationManager, loadedVariables);
    }
}