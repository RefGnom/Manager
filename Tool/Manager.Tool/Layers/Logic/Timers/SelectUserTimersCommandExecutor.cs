using System.Threading.Tasks;
using Manager.Core.Common.Enum;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class SelectUserTimersCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    ILogger<SelectUserTimersCommand> logger
) : CommandExecutorBase<SelectUserTimersCommand>(toolCommandFactory, logger)
{
    private readonly ILogger<SelectUserTimersCommand> logger = logger;

    protected override async Task ExecuteAsync(CommandContext context, SelectUserTimersCommand command)
    {
        var withDeleted = context.ContainsOption("--with_deleted");
        var withArchived = context.ContainsOption("--with_archived");

        var userTimersRequest = timerRequestFactory.CreateUserTimersRequest(
            context.EnsureUser().Id,
            withDeleted,
            withArchived
        );
        var userTimersResponse = await timerServiceApiClient.SelectUserTimersAsync(userTimersRequest);

        if (userTimersResponse.Timers.Length == 0)
        {
            logger.WriteMessage("У вас нет таймеров");
            return;
        }

        var timers = userTimersResponse.Timers;
        for (var i = 0; i < timers.Length; i++)
        {
            var timer = timers[i];
            logger.WriteMessage(
                $"{NormalizeTimerNumber(i + 1, timers.Length)}. {timer.Name}, {timer.TimerStatus.GetDescription()}"
            );
        }
    }

    private string NormalizeTimerNumber(int number, int timersCount)
    {
        var numberLength = timersCount.ToString().Length;
        return number.ToString().PadLeft(numberLength);
    }
}