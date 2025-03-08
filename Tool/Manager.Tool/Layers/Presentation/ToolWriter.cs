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

    public void WriteMessageWithIndent(int indentLevel, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[] args)
    {
        var indent = CreateIndent(indentLevel);
        var text = string.Format(message, args);
        Console.WriteLine(indent + text);
    }

    public void WriteToolCommand(IToolCommand command, bool isDetailed = false)
    {
        const int commandNameLength = 12;
        const int optionKeysLength = 24;

        WriteMessageWithIndent(1, "{0}{1}", command.CommandName.PadRight(commandNameLength), command.Description);
        if (!isDetailed || command.CommandOptions.Length == 0)
        {
            return;
        }

        foreach (var option in command.CommandOptions)
        {
            var optionKeys = new[] { option.ShortKey, option.FullKey }
                .Where(x => x is not null)
                .JoinToString(", ")
                .PadRight(optionKeysLength);

            WriteMessageWithIndent(2, "{0}{1}", optionKeys, option.Description);
        }
    }

    public void MoveOnNextLine()
    {
        Console.WriteLine();
    }

    private string CreateIndent(int indentLevel)
    {
        return new string(' ', _indentSize * indentLevel);
    }
}