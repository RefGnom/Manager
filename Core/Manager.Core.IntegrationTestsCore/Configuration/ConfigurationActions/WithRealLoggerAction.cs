using System.Collections.Generic;
using System.Net;
using Manager.Core.Common;
using Manager.Core.Logging.Configuration;
using Manager.Core.Telemetry;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manager.Core.IntegrationTestsCore.Configuration.ConfigurationActions;

public class WithRealLoggerAction : IConfigurationAction
{
    public ConfigurationActionType Type { get; } = ConfigurationActionType.WithRealLogger;
    public ConfigurationActionType[] ExcludedTypes { get; } = [ConfigurationActionType.WithNullLogger];

    public void Invoke(ConfigurationActionContext context)
    {
        var resources = new Dictionary<string, object>
        {
            ["service.name"] = "IntegrationTests",
            ["HostName"] = Dns.GetHostName(),
            ["environment"] = Environments.Testing,
        };

        context.ServiceCollection
            .AddLogging(x => x.AddConsole())
            .AddCustomLogger(
                context.ConfigurationManager,
                Environments.Testing,
                OpenTelemetryLogWriteStrategyFactory.CreateWithResources(resources)
            );
    }
}