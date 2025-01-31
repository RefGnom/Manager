namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface ICommandContextFactory
{
    CommandContext Create(string[] arguments);
}