using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Manager.Tool.Layers.Logic;

public static class LoggerExtensions
{
    public static void WriteMessage(this ILogger logger, string message)
    {
        logger.WriteMessageWithIndent(message, 0);
    }

    public static void WriteMessageWithIndent(this ILogger logger, string message, int indentLevel)
    {
        var indent = new string(' ', indentLevel);
        logger.LogInformation("{indent}{userMessage}", indent, message);
    }

    public static void WriteToolCommand(this ILogger logger, IToolCommand command, bool isDetailed = false)
    {
        var commandDescription = isDetailed
            ? JsonConvert.SerializeObject(command)
            : $"{command.CommandName} {command.Description}";
        logger.LogInformation("{command}", commandDescription);
    }

    public static void MoveOnNextLine(this ILogger logger)
    {
        logger.LogInformation("");
    }
}