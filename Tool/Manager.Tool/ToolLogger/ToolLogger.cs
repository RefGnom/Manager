using Manager.Core.Logger;

namespace Manager.Tool.ToolLogger;

public class ToolLogger<TContext>(ILogger<TContext> logger) : IToolLogger<TContext>
{
    private readonly ILogger<TContext> _logger = logger;

    public void LogInfo(bool isDebugMode, string text, params object?[] args)
    {
        if (isDebugMode)
        {
            _logger.LogInfo(text, args);
        }
    }

    public void LogWarn(bool isDebugMode, string text, params object?[] args)
    {
        if (isDebugMode)
        {
            _logger.LogWarn(text, args);
        }
    }

    public void LogError(bool isDebugMode, string text, params object?[] args)
    {
        if (isDebugMode)
        {
            _logger.LogError(text, args);
        }
    }
}