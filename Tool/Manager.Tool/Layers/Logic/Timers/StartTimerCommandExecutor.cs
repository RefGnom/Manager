using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IUserTimeService userTimeService,
    ILogger<StartTimerCommand> logger
) : CommandExecutorBase<StartTimerCommand>(toolCommandFactory, logger)
{
    private readonly ILogger<StartTimerCommand> logger = logger;

    protected override async Task ExecuteAsync(CommandContext context, StartTimerCommand command)
    {
        var user = context.EnsureUser();
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;
        var startTimeResult = context.GetDateTimeOptionValue(command.StartTimeOption) ?? userTimeService.GetUserTime(user);
        var pingTimeoutResult = context.GetTimeSpanOptionValue(command.PingTimeoutOption);

        if (!startTimeResult.IsSuccess)
        {
            logger.WriteMessage(startTimeResult.Error);
            return;
        }

        if (pingTimeoutResult?.IsSuccess == false)
        {
            logger.WriteMessage(pingTimeoutResult.Error);
            return;
        }

        var startTimerRequest = timerRequestFactory.CreateStartTimerRequest(user.Id, timerName, startTimeResult.Value, pingTimeoutResult?.Value);
        var startTimerResponse = await timerServiceApiClient.StartTimerAsync(startTimerRequest);

        if (startTimerResponse.IsSuccessStatusCode)
        {
            logger.WriteMessage("Таймер успешно запущен");
            return;
        }

        logger.WriteMessage(startTimerResponse.ResponseMessage ?? "Неизвестная ошибка");
    }
}