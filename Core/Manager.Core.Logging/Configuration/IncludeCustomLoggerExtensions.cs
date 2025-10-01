using System;
using Manager.Core.Common.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace Manager.Core.Logging.Configuration;

public static class IncludeCustomLoggerExtensions
{
    public const bool Dispose = true;
    public const string StartupLoggerContext = "Startup";
    private const string SettingsFileName = "loggingsettings";

    /// <summary>
    /// Регистрирует Serilog логгер.
    /// Использует конфигурацию по умолчанию из файлов loggingsettings.json и loggingsettings.Development.json
    /// Конфигурацию можно переопределять в настройках своего сервиса
    /// <br /><br />
    /// Важно. Если логгера в вашем сервисе вовсе нет, то его нужно зарегистрировать с помощью
    /// serviceCollection.AddLogging(x => x.AddConsole())
    /// </summary>
    public static IHostApplicationBuilder AddCustomLogger(this IHostApplicationBuilder builder)
    {
        builder.Services.AddCustomLogger(builder.Configuration, builder.Environment.EnvironmentName);

        return builder;
    }

    /// <summary>
    /// Регистрирует Serilog логгер.
    /// Использует конфигурацию по умолчанию из файлов loggingsettings.json и loggingsettings.Development.json
    /// Конфигурацию можно переопределять в настройках своего сервиса
    /// <br /><br />
    /// Важно. Если логгера в вашем сервисе вовсе нет, то его нужно зарегистрировать с помощью
    /// serviceCollection.AddLogging(x => x.AddConsole())
    /// </summary>
    public static IServiceCollection AddCustomLogger(
        this IServiceCollection services,
        IConfigurationManager configuration,
        string environmentName
    )
    {
        AddCustomLoggerConfiguration(configuration, environmentName);

        var startupLogger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger()
            .ForContext(Constants.SourceContextPropertyName, StartupLoggerContext);
        Log.Logger = startupLogger;

        startupLogger.Information("Configuration logging");
        services.AddSerilog(dispose: Dispose);

        return services;
    }

    private static void AddCustomLoggerConfiguration(this IConfigurationBuilder configuration, string environment)
    {
        var fileProvider = new PhysicalFileProvider(AppContext.BaseDirectory);
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
}