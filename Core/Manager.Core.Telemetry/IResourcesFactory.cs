using System.Collections.Generic;
using System.Net;
using Manager.Core.Common;
using Microsoft.Extensions.Hosting;

namespace Manager.Core.Telemetry;

public interface IResourcesFactory
{
    Dictionary<string, object> Create();
}

public class HostAppResourcesFactory(
    IHostEnvironment hostEnvironment
) : IResourcesFactory
{
    public Dictionary<string, object> Create() => new()
    {
        ["service.name"] = ManagerApp.FriendlyName,
        ["HostName"] = Dns.GetHostName(),
        ["environment"] = hostEnvironment.EnvironmentName,
    };
}