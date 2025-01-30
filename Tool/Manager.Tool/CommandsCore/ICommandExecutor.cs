namespace Manager.Tool.CommandsCore;

public interface ICommandExecutor
{
    bool CanExecute(CommandOptions commandOptions);
    Task ExecuteAsync(CommandOptions commandOptions);
}