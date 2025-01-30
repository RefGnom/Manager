namespace Manager.Core.Logger;

public interface ILogger<TContext>
{
    public void LogInfo(string text);
    public void LogWarn(string text);
    public void LogError(string text);
}