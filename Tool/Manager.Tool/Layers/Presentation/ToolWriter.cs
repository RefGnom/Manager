using System;

namespace Manager.Tool.Layers.Presentation;

public class ToolWriter : IToolWriter
{
    public void WriteMessage(string message, params object?[] args)
    {
        var text = string.Format(message, args);
        Console.WriteLine(text);
    }
}