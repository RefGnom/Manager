using Manager.Core.DateTimeProvider;

namespace Manager.Core.Logger;

public class Logger<TContext>(IDateTimeProvider dateTimeProvider) : ILogger<TContext>
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;


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
        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();

        Console.ForegroundColor = color;
        Console.WriteLine($"{currentDateTime} [{typeof(TContext).Name}] [{logType}] {textWithArgs}");
        Console.ResetColor();
    }
}