using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Manager.Core.Logging.Configuration;

public static class StartupLoggerFactory
{
    /// <summary>
    ///     Позволяет получить логера во время конфигурации
    ///     <br /><br />
    ///     Важно! Если Serilog логер не был зарегистрирован, то воспользоваться логером не получится
    /// </summary>
    public static ILogger CreateStartupLogger() =>
        new SerilogLoggerFactory(Log.Logger, IncludeCustomLoggerExtensions.Dispose)
            .CreateLogger(IncludeCustomLoggerExtensions.StartupLoggerContext);
}