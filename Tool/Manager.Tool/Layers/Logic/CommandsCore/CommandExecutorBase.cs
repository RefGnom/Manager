using System.Threading.Tasks;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public abstract class CommandExecutorBase<TCommand>(IToolCommandFactory toolCommandFactory) : ICommandExecutor
    where TCommand : IToolCommand, new()
{
    private readonly IToolCommandFactory _toolCommandFactory = toolCommandFactory;

    public virtual bool CanExecute(CommandContext context)
    {
        var command = _toolCommandFactory.CreateCommand<TCommand>();

        if (!context.CommandSpace.Equals(command.CommandSpace))
        {
            return false;
        }

        return context.CommandName == command.CommandName;
    }

    public abstract Task ExecuteAsync(CommandContext context);
}