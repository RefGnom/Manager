using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class StopTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IUserTimeService userTimeService,
    ILogger<StopTimerCommand> logger
) : CommandExecutorBase<StopTimerCommand>(toolCommandFactory, logger)
{
    private readonly ILogger<StopTimerCommand> logger = logger;

    protected override async Task ExecuteAsync(CommandContext context, StopTimerCommand command)
    {
        var user = context.EnsureUser();
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;
        var stopTimeResult = context.GetDateTimeOptionValue(command.StopTimeOption)
                             ?? userTimeService.GetUserTime(user);

        if (!stopTimeResult.IsSuccess)
        {
            logger.WriteMessage(stopTimeResult.Error);
            return;
        }

        var stopTimerRequest = timerRequestFactory.CreateStopTimerRequest(user.Id, timerName, stopTimeResult.Value);
        var stopTimerResponse = await timerServiceApiClient.StopTimerAsync(stopTimerRequest);

        if (stopTimerResponse.IsSuccessStatusCode)
        {
            logger.WriteMessage("Таймер успешно остановлен");
            return;
        }

        logger.WriteMessage(stopTimerResponse.ResponseMessage ?? "Неизвестная ошибка");
    }
}