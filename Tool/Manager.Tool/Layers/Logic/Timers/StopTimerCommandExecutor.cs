using System.Threading.Tasks;
using Manager.Core.Common.Linq;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class StopTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ILogger<StopTimerCommandExecutor> logger
) : CommandExecutorBase<StopTimerCommand>(toolCommandFactory)
{
    public override Task ExecuteAsync(CommandContext context)
    {
        logger.LogInformation(
            "Выполняем команду {command} с аргументами {options}",
            context.Arguments,
            context.Options.JoinToString(", ")
        );
        return Task.CompletedTask;
    }
}