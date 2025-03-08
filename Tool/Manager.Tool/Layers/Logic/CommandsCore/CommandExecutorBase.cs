using System.Linq;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.ToolLogger;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public abstract class CommandExecutorBase<TCommand>(
    IToolCommandFactory toolCommandFactory,
    IToolLogger<TCommand> logger
) : ICommandExecutor
    where TCommand : IToolCommand, new()
{
    private readonly IToolCommandFactory _toolCommandFactory = toolCommandFactory;
    private readonly IToolLogger<TCommand> _logger = logger;

    public virtual bool CanExecute(CommandContext context)
    {
        var command = _toolCommandFactory.CreateCommand<TCommand>();

        var spaceLength = command.CommandSpace?.Values.Length ?? 0;
        var argumentsSpace = context.Arguments.Take(spaceLength).ToArray();
        if (command.CommandSpace is not null && !command.CommandSpace.Values.SequenceEqual(argumentsSpace))
        {
            return false;
        }

        var commandName = context.Arguments.Skip(spaceLength).FirstOrDefault();
        return command.CommandName == commandName;
    }

    public Task ExecuteAsync(CommandContext context)
    {
        _logger.LogInfo(context.IsDebugMode, "Начинаем выполнение команды");
        var command = _toolCommandFactory.CreateCommand<TCommand>();
        return ExecuteAsync(context, command);
    }

    protected abstract Task ExecuteAsync(CommandContext context, TCommand command);
}