namespace Manager.Core.Logger;

public interface ILogger<TContext>
{
    public void LogInfo(string text, params object?[] args);
    public void LogWarn(string text, params object?[] args);
    public void LogError(string text, params object?[] args);
}