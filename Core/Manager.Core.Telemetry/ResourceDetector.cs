using OpenTelemetry.Resources;

namespace Manager.Core.Telemetry;

public class ResourceDetector(
    IResourcesFactory resourcesFactory
) : IResourceDetector
{
    public Resource Detect() => new(resourcesFactory.Create());
}