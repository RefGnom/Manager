namespace Manager.Tool.CommandsCore;

public class CommandOptions(string[] parameters)
{
    public string[] Parameters { get; } = parameters;
    public bool IsDebugMode { get; } = parameters.Contains("-d");

    public string this[int index] => Parameters[index];
}