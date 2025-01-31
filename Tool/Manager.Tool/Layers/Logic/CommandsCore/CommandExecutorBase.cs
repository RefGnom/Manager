namespace Manager.Tool.Layers.Logic.CommandsCore;

public abstract class CommandExecutorBase<TCommand>(IToolCommandFactory toolCommandFactory) : ICommandExecutor
    where TCommand : IToolCommand, new()
{
    private readonly IToolCommandFactory _toolCommandFactory = toolCommandFactory;

    public virtual bool CanExecute(CommandContext context)
    {
        var command = _toolCommandFactory.CreateCommand<TCommand>();

        if (command.CommandSpace is null)
        {
            return context.Options[0].Argument == command.Command;
        }

        return context.Options[0].Argument == command.CommandSpace && context.Options[1].Argument == command.Command;
    }

    public abstract Task ExecuteAsync(CommandContext context);
}