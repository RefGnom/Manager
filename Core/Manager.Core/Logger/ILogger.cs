using System.Diagnostics.CodeAnalysis;

namespace Manager.Core.Logger;

public interface ILogger<TContext>
{
    public void LogInfo([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args);
    public void LogWarn([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args);
    public void LogError([StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args);
}