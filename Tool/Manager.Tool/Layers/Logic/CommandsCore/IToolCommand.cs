namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface IToolCommand
{
    string CommandName { get; }
    string Description { get; }
    CommandSpace? CommandSpace { get; }
    CommandOptionInfo[] CommandOptions => [];
}