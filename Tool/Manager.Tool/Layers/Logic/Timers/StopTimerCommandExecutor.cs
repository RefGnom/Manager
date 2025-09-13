using System.Threading.Tasks;
using Manager.Core.Extensions.LinqExtensions;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Logic.ToolLogger;

namespace Manager.Tool.Layers.Logic.Timers;

public class StopTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    IToolLogger<StopTimerCommandExecutor> logger
) : CommandExecutorBase<StopTimerCommand>(toolCommandFactory)
{
    public override Task ExecuteAsync(CommandContext context)
    {
        logger.LogInfo(
            context.IsDebugMode,
            "Выполняем команду {0} с аргументами {1}",
            context.Arguments,
            context.Options.JoinToString(", ")
        );
        return Task.CompletedTask;
    }
}