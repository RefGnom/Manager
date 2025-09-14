using System;
using Manager.Core.Common.Time;

namespace Manager.Core.Logger;

public class ConsoleLogger<TContext>(
    IDateTimeProvider dateTimeProvider) : ILogger<TContext>
{
    public void LogInfo(string text, params object?[] args)
    {
        Log("Info", text, args, ConsoleColor.Gray);
    }

    public void LogWarn(string text, params object?[] args)
    {
        Log("Warn", text, args, ConsoleColor.Yellow);
    }

    public void LogError(string text, params object?[] args)
    {
        Log("Info", text, args, ConsoleColor.Red);
    }

    private void Log(string logType, string text, object?[] args, ConsoleColor color)
    {
        var textWithArgs = string.Format(text, args);
        var currentDateTime = dateTimeProvider.Now;

        Console.ForegroundColor = color;
        Console.WriteLine($"{currentDateTime} [{typeof(TContext).Name}] [{logType}] {textWithArgs}");
        Console.ResetColor();
    }
}