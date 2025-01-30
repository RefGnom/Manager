namespace Manager.Tool.CommandsCore;

public interface ICommandContextFactory
{
    CommandContext Create(string[] arguments);
}