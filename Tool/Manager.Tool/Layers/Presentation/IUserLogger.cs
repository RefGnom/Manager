using System.Diagnostics.CodeAnalysis;

namespace Manager.Tool.Layers.Presentation;

public interface IUserLogger
{
    void LogUserMessage([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string message, params object?[] args);
}