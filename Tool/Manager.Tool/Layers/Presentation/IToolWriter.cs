using System.Diagnostics.CodeAnalysis;
using Manager.Tool.Layers.Logic.CommandsCore;

namespace Manager.Tool.Layers.Presentation;

public interface IToolWriter
{
    void WriteMessage([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[] args);
    void WriteMessageWithIndent(int indentLevel, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[] args);
    void WriteToolCommand(IToolCommand command, bool isDetailed = false);
    void MoveOnNextLine();
}