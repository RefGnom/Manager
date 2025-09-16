using System;
using Microsoft.Extensions.Hosting;
using static Manager.Tool.Configuration.Environment;

namespace Manager.Tool.Configuration;

public static class EnvironmentConverter
{
    public static string ConvertToAspEnvironment(string environment)
    {
        return environment switch
        {
            Debug => Environments.Development,
            Information => Environments.Production,
            _ => throw new NotSupportedException($"Unknown environment {environment}"),
        };
    }
}