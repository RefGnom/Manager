using System;
using System.Net;
using Manager.Core.Common;
using Manager.Core.Logging.Configuration;
using Serilog;
using Serilog.Configuration;
using Serilog.Sinks.OpenTelemetry;

namespace Manager.Core.Telemetry;

public class OpenTelemetryLogWriteStrategy : CustomWriteStrategy
{
    public override LoggerConfiguration Apply(LoggerSinkConfiguration writeTo) =>
        writeTo.OpenTelemetry(ConfigureOpenTelemetry);

    private static void ConfigureOpenTelemetry(OpenTelemetrySinkOptions options)
    {
        options.Endpoint = TelemetryOptions.EndPoint;
        options.Protocol = (OtlpProtocol)TelemetryOptions.Protocol;
        options.ResourceAttributes["service.name"] = ManagerApp.FriendlyName;
        options.ResourceAttributes["host.name"] = Environment.MachineName;
        options.ResourceAttributes["host.name.dns"] = Dns.GetHostName();
    }
}