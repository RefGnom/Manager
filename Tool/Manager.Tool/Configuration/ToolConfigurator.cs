using System;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.Logging.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Configuration;

public static class ToolConfigurator
{
    public static IConfigurationManager CreateSettingsConfiguration()
    {
        var configuration = new ConfigurationManager();
        configuration.AddJsonFile("appsettings.json");
        configuration.AddJsonFile($"appsettings.{Environment.CurrentEnvironment}.json", optional: true);

        return configuration;
    }

    public static IServiceProvider CreateServiceProvider(this IConfigurationManager configurationManager)
    {
        var serviceCollection = new ServiceCollection()
            .AddLogging(x => x.AddConsole())
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon();

        var aspEnvironment = EnvironmentConverter.ConvertToAspEnvironment(Environment.CurrentEnvironment);
        serviceCollection.AddCustomLogger(configurationManager, aspEnvironment);

        return serviceCollection.BuildServiceProvider();
    }
}