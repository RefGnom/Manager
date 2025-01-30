namespace Manager.Tool.CommandsCore;

public interface IToolCommand
{
    string Command { get; }
    string? CommandSpace => null;
}