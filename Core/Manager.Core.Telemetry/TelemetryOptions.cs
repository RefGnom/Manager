using OpenTelemetry.Exporter;

namespace Manager.Core.Telemetry;

public static class TelemetryOptions
{
    public static readonly OtlpExportProtocol Protocol = OtlpExportProtocol.HttpProtobuf;
    public static readonly string EndPoint = "http://147.45.150.159:4318";
}