using System.Linq;
using System.Threading.Tasks;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public abstract class CommandExecutorBase<TCommand>(
    IToolCommandFactory toolCommandFactory
) : ICommandExecutor where TCommand : IToolCommand, new()
{
    public virtual bool CanExecute(CommandContext context)
    {
        var command = toolCommandFactory.CreateCommand<TCommand>();

        var spaceLength = command.CommandSpace.Values.Length;
        var argumentsSpace = (CommandSpace)context.Arguments.Take(spaceLength).ToArray();
        if (!command.CommandSpace.Equals(argumentsSpace))
        {
            return false;
        }

        var commandName = context.Arguments.Skip(spaceLength).FirstOrDefault();
        return command.CommandName == commandName;
    }

    public abstract Task ExecuteAsync(CommandContext context);
}