using OpenTelemetry.Exporter;

namespace Manager.Core.Telemetry;

public static class TelemetryOptions
{
    public const OtlpExportProtocol Protocol = OtlpExportProtocol.HttpProtobuf;
    public const string EndPoint = "http://147.45.150.159:4318";
}