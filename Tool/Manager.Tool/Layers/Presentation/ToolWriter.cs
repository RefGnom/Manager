using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Manager.Core.Extensions.LinqExtensions;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Presentation;

public class ToolWriter : IToolWriter
{
    private const int _indentSize = 4;

    public void WriteMessage([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[] args)
    {
        var text = string.Format(message, args);
        Console.WriteLine(text);
    }

    public void WriteMessageWithIndent(int indentLevel, string message, params object?[] args)
    {
        var indent = CreateIndent(indentLevel);
        var text = string.Format(message, args);
        Console.WriteLine(indent + text);
    }

    public void WriteToolCommand(IToolCommand command, bool isDetailed = false)
    {
        // todo: Добавить вывод описания команды
        WriteMessageWithIndent(1, command.CommandName);
        if (!isDetailed)
        {
            return;
        }

        const int optionKeysLength = 24;
        foreach (var option in command.CommandOptions)
        {
            var optionKeys = new[] { option.ShortKey, option.FullKey }
                .Where(x => x is not null)
                .JoinToString(", ")
                .PadRight(optionKeysLength);

            WriteMessageWithIndent(2, "{0}{1}", optionKeys.PadLeft(_indentSize * 2), option.Description);
        }
    }

    private string CreateIndent(int indentLevel)
    {
        return new string(' ', _indentSize * indentLevel);
    }
}