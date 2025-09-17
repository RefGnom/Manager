using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic;

public static class LoggerExtensions
{
    public static void WriteMessage(this ILogger logger, string message)
    {
        logger.WriteMessageWithIndent(message, 0);
    }

    public static void WriteMessageWithIndent(this ILogger logger, string message, int indentLevel)
    {
        logger.LogInformation("{indent}{userMessage}", indentLevel, message);
    }

    public static void WriteToolCommand(this ILogger logger, IToolCommand command, bool isDetailed = false)
    {
        logger.LogInformation("{tool}", command);
    }

    public static void MoveOnNextLine(this ILogger logger)
    {
        logger.LogInformation("");
    }
}