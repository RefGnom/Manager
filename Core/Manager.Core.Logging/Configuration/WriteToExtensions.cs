using Serilog;
using Serilog.Configuration;

namespace Manager.Core.Logging.Configuration;

public static class WriteToExtensions
{
    public static LoggerConfiguration Custom(
        this LoggerSinkConfiguration loggerSinkConfiguration,
        CustomWriteStrategy action
    ) => action.Apply(loggerSinkConfiguration);
}