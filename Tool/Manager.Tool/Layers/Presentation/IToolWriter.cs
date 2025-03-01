using System.Diagnostics.CodeAnalysis;

namespace Manager.Tool.Layers.Presentation;

public interface IToolWriter
{
    void WriteMessage([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[] args);
}