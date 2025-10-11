using Manager.Core.Logging.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithRealLoggerAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithRealLogger;
    public ConfigurationActionType[] ExcludedTypes { get; } = [ConfigurationActionType.WithNullLogger];

    public void Invoke(ConfigurationActionContext context)
    {
        context.ServiceCollection
            .AddLogging(x => x.AddConsole())
            .AddCustomLogger(context.ConfigurationManager, Environments.Development);
    }
}