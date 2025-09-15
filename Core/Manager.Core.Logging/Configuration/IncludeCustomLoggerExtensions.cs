using System;
using System.IO;
using Manager.Core.Common.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;
using Serilog.Extensions.Logging;
using IMicrosoftLogger = Microsoft.Extensions.Logging.ILogger;

namespace Manager.Core.Logging.Configuration;

public static class IncludeCustomLoggerExtensions
{
    private const bool Dispose = true;
    private const string StartupLoggerContext = "Startup";
    private const string SettingsFileName = "loggingsettings";

    public static IMicrosoftLogger AddCustomLogger(this IHostApplicationBuilder builder)
    {
        AddCustomLoggerConfiguration(builder.Configuration, builder.Environment.EnvironmentName);

        var startupLogger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger()
            .ForContext(Constants.SourceContextPropertyName, StartupLoggerContext);
        Log.Logger = startupLogger;

        startupLogger.Information("Configuration logging");
        builder.Services.AddSerilog(dispose: Dispose);
        return new SerilogLoggerFactory(startupLogger, Dispose).CreateLogger(StartupLoggerContext);
    }

    private static void AddCustomLoggerConfiguration(this IConfigurationBuilder configuration, string environment)
    {
        var fileProvider = GetSettingsFileProvider();
        // Порядок важен, приоритет определяется сверху вниз. Сначала настройки для окружения, потом общие
        AddJsonConfiguration(configuration, fileProvider, $"{SettingsFileName}.{environment}.json");
        AddJsonConfiguration(configuration, fileProvider, $"{SettingsFileName}.json", false);
    }

    private static void AddJsonConfiguration(
        IConfigurationBuilder configuration,
        PhysicalFileProvider fileProvider,
        string filename,
        bool optional = true
    )
    {
        var configurationSource = new JsonConfigurationSource
        {
            Optional = optional,
            Path = filename,
            ReloadDelay = 5000,
            ReloadOnChange = true,
            FileProvider = fileProvider,
        };

        configuration.Sources.InsertBefore(x => x is JsonConfigurationSource, configurationSource);
    }

    private static PhysicalFileProvider GetSettingsFileProvider()
    {
        var relativeSettingsDirectoryPath = Path.Combine(
            AppContext.BaseDirectory,
            "..",
            "..",
            "..",
            "..",
            "..",
            "Core",
            "Manager.Core.Logging"
        );
        var absoluteSettingsDirectoryPath = Path.GetFullPath(relativeSettingsDirectoryPath);
        return new PhysicalFileProvider(absoluteSettingsDirectoryPath);
    }
}