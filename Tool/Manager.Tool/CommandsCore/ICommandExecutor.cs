namespace Manager.Tool.CommandsCore;

public interface ICommandExecutor
{
    bool CanExecute(CommandContext context);
    Task ExecuteAsync(CommandContext context);
}