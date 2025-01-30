namespace Manager.Tool.CommandsCore;

public abstract class CommandExecutorBase<TCommand>(IToolCommandFactory toolCommandFactory) : ICommandExecutor
    where TCommand : IToolCommand, new()
{
    private readonly IToolCommandFactory _toolCommandFactory = toolCommandFactory;

    public virtual bool CanExecute(CommandOptions commandOptions)
    {
        var command = _toolCommandFactory.CreateCommand<TCommand>();

        if (command.CommandSpace is null)
        {
            return commandOptions[0] == command.Command;
        }

        return commandOptions[0] == command.CommandSpace && commandOptions[1] == command.Command;
    }

    public abstract Task ExecuteAsync(CommandOptions commandOptions);
}