using System;

namespace Manager.Core.AppConfiguration;

[AttributeUsage(AttributeTargets.Assembly)]
public class ServerPropertiesAttribute(
    string portKey,
    string dockerContainerName
) : Attribute
{
    public string PortKey { get; init; } = portKey;
    public string DockerContainerName { get; init; } = dockerContainerName;
}