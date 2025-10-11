using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithNullLoggerAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithNullLogger;
    public ConfigurationActionType[] ExcludedTypes { get; } = [ConfigurationActionType.WithRealLogger];

    public void Invoke(ConfigurationActionContext context)
    {
        context.ServiceCollection
            .AddSingleton<ILogger, NullLogger>()
            .AddSingleton(typeof(ILogger<>), typeof(NullLogger<>));
    }
}