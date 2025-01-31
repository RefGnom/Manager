using System.Diagnostics.CodeAnalysis;

namespace Manager.Tool.Layers.Logic.ToolLogger;

public interface IToolLogger<TContext>
{
    public void LogInfo(bool isDebugMode, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args);
    public void LogWarn(bool isDebugMode, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args);
    public void LogError(bool isDebugMode, [StringSyntax(StringSyntaxAttribute.CompositeFormat)] string text, params object?[] args);
}