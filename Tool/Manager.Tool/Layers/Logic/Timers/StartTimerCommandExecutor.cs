using System.Threading.Tasks;
using Manager.Core.Extensions.LinqExtensions;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IToolLogger<StartTimerCommandExecutor> logger
) : CommandExecutorBase<StartTimerCommand>(toolCommandFactory)
{
    private readonly IToolLogger<StartTimerCommandExecutor> _logger = logger;

    protected override Task ExecuteAsync(CommandContext context, StartTimerCommand command)
    {
        _logger.LogInfo(
            context.IsDebugMode,
            "Выполняем команду {0} с аргументами {1}",
            context.Arguments,
            context.Options.JoinToString(", ")
        );
        return Task.CompletedTask;
    }
}