namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface IToolCommand
{
    string Command { get; }
    string? CommandSpace => null;
}