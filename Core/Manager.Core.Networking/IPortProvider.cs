using System;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.Networking;

public interface IPortProvider
{
    string GetPort(string applicationPortKey);
}

public class PortProvider(
    IConfiguration configuration
) : IPortProvider
{
    public string GetPort(string applicationPortKey) =>
        configuration.GetValue<string>(applicationPortKey) ??
        throw new Exception($"Port by key {applicationPortKey} not configured");
}