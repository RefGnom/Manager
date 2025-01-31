using Manager.Core.LinqExtensions;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimeCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IToolLogger<StartTimeCommandExecutor> logger
)
    : CommandExecutorBase<StartTimerCommand>(toolCommandFactory)
{
    private readonly IToolLogger<StartTimeCommandExecutor> _logger = logger;

    public override Task ExecuteAsync(CommandContext context)
    {
        _logger.LogInfo(
            context.IsDebugMode,
            "Выполняем команду {0} с аргументами {1}",
            context.Options[1].Argument,
            context.Options.Skip(2).JoinToString(", ")
        );
        return Task.CompletedTask;
    }
}