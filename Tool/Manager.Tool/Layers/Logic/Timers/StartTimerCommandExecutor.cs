using System.Threading.Tasks;
using Manager.Core.Common.Linq;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ILogger<StartTimerCommandExecutor> logger
) : CommandExecutorBase<StartTimerCommand>(toolCommandFactory)
{
    public override Task ExecuteAsync(CommandContext context)
    {
        logger.LogDebug(
            "Выполняем команду {command} с аргументами {options}",
            context.Arguments,
            context.Options.JoinToString(", ")
        );
        return Task.CompletedTask;
    }
}