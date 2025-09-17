using Serilog;
using Serilog.Extensions.Logging;
using ILogger = Microsoft.Extensions.Logging.ILogger;

namespace Manager.Core.Logging.Configuration;

public static class StartupLoggerFactory
{
    public static ILogger CreateStartupLogger()
    {
        return new SerilogLoggerFactory(Log.Logger, IncludeCustomLoggerExtensions.Dispose)
            .CreateLogger(IncludeCustomLoggerExtensions.StartupLoggerContext);
    }
}