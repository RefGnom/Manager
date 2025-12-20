using System.Collections.Generic;
using Manager.Core.Logging.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.OpenTelemetry;

namespace Manager.Core.Telemetry;

public class OpenTelemetryLogWriteStrategy(
    IResourcesFactory resourcesFactory
) : CustomWriteStrategy
{
    public override LoggerConfiguration Apply(LoggerSinkConfiguration writeTo) =>
        writeTo.OpenTelemetry(ConfigureOpenTelemetry);

    private void ConfigureOpenTelemetry(OpenTelemetrySinkOptions options)
    {
        options.Endpoint = TelemetryOptions.EndPoint;
        options.Protocol = (OtlpProtocol)TelemetryOptions.Protocol;
        options.ResourceAttributes = resourcesFactory.Create();
    }
}

public static class OpenTelemetryLogWriteStrategyFactory
{
    public static OpenTelemetryLogWriteStrategy CreateForHostApp(IHostApplicationBuilder hostApplicationBuilder) =>
        new(new HostAppResourcesFactory(hostApplicationBuilder.Environment));

    public static OpenTelemetryLogWriteStrategy CreateWithResources(Dictionary<string, object> resources) =>
        new(new StaticResourcesFactory(resources));
}