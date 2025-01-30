namespace Manager.Tool.CommandsCore;

public interface ICommandExecutorProvider
{
    ICommandExecutor? GetByContext(CommandContext context);
}