using System;

namespace Manager.Tool.Layers.Presentation;

public class UserLogger : IUserLogger
{
    public void LogUserMessage(string message, params object?[] args)
    {
        var text = string.Format(message, args);
        Console.WriteLine(text);
    }
}