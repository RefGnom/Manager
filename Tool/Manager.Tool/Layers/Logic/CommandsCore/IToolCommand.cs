namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface IToolCommand
{
    string CommandName { get; }
    CommandSpace CommandSpace => CommandSpace.Empty;
    CommandOptionInfo[] CommandOptions => [];
}