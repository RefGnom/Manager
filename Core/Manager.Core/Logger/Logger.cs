using Manager.Core.DateTimeProvider;

namespace Manager.Core.Logger;

public class Logger<TContext>(IDateTimeProvider dateTimeProvider) : ILogger<TContext>
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public void LogInfo(string text)
    {
        Log("Info", text, ConsoleColor.Gray);
    }

    public void LogWarn(string text)
    {
        Log("Warn", text, ConsoleColor.Yellow);
    }

    public void LogError(string text)
    {
        Log("Info", text, ConsoleColor.Red);
    }

    private void Log(string logType, string text, ConsoleColor color)
    {
        var currentDateTime = _dateTimeProvider.GetCurrentDateTime();

        Console.ForegroundColor = color;
        Console.WriteLine($"{currentDateTime} [{typeof(TContext).Name}] [{logType}] {text}");
        Console.ResetColor();
    }
}