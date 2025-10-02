using System;
using System.Collections.Generic;
using System.Text;
using Manager.Core.Common.Linq;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic;

public static class LoggerExtensions
{
    private const string ToolName = "manager";

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
            ? GetDetailedCommandDescription(command)
            : $"{command.CommandName} {command.Description}";
        logger.LogInformation("{command}", commandDescription);
    }

    public static void MoveOnNextLine(this ILogger logger)
    {
        logger.LogInformation("");
    }

    private static string GetDetailedCommandDescription(IToolCommand command)
    {
        var sb = new StringBuilder();
        sb.Append(ToolName);
        var commandPath = new List<string> { ToolName };
        commandPath.AddRange(command.CommandSpace?.Values ?? []);
        commandPath.Add(command.CommandName);
        sb.Append(commandPath.JoinToString(" ").PadRight(40));
        sb.Append(command.Description);
        if (command.CommandOptions.Length > 0)
        {
            sb.AppendLine($"{Environment.NewLine}Options:");
        }

        foreach (var option in command.CommandOptions)
        {
            var shortKey = option.ShortKey ?? "";
            var fullKey = option.FullKey ?? "";
            sb.AppendLine($"{new string(' ', 4)}{shortKey.PadRight(8)}{fullKey.PadRight(20)}{option.Description}");
        }

        return sb.ToString();
    }
}