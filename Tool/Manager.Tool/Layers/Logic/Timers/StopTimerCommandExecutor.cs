using System.Threading.Tasks;
using Manager.Core.LinqExtensions;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;

namespace Manager.Tool.Layers.Logic.Timers;

public class StopTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IToolLogger<StopTimerCommandExecutor> logger
) : CommandExecutorBase<StopTimerCommand>(toolCommandFactory)
{
    private readonly IToolLogger<StopTimerCommandExecutor> _logger = logger;

    public override Task ExecuteAsync(CommandContext context)
    {
        _logger.LogInfo(
            context.IsDebugMode,
            "Выполняем команду {0} с аргументами {1}",
            context.CommandName,
            context.Flags.JoinToString(", ")
        );
        return Task.CompletedTask;
    }
}