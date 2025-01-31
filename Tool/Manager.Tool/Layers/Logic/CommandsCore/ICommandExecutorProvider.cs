namespace Manager.Tool.Layers.Logic.CommandsCore;

public interface ICommandExecutorProvider
{
    ICommandExecutor? GetByContext(CommandContext context);
}