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

    public bool CanExecute(IToolCommand command)
    {
        return command is TCommand;
    }

    public Task ExecuteAsync(CommandContext context)
    {
        _logger.LogInfo(context.IsDebugMode, "Начинаем выполнение команды");
        var command = _toolCommandFactory.CreateCommand<TCommand>();
        return ExecuteAsync(context, command);
    }

    protected abstract Task ExecuteAsync(CommandContext context, TCommand command);
}