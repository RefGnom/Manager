using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public abstract class CommandExecutorBase<TCommand>(
    IToolCommandFactory toolCommandFactory,
    ILogger<TCommand> logger
) : ICommandExecutor
    where TCommand : IToolCommand, new()
{
    public bool CanExecute(IToolCommand command) => command is TCommand;

    public Task ExecuteAsync(CommandContext context)
    {
        logger.LogDebug("Начинаем выполнение команды");
        var command = toolCommandFactory.CreateCommand<TCommand>();
        return ExecuteAsync(context, command);
    }

    protected abstract Task ExecuteAsync(CommandContext context, TCommand command);
}