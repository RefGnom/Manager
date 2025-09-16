using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter,
    IUserTimeService userTimeService,
    ILogger<StartTimerCommand> logger
) : CommandExecutorBase<StartTimerCommand>(toolCommandFactory, logger)
{
    protected override async Task ExecuteAsync(CommandContext context, StartTimerCommand command)
    {
        var user = context.EnsureUser();
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;
        var startTimeResult = context.GetDateTimeOptionValue(command.StartTimeOption) ?? userTimeService.GetUserTime(user);
        var pingTimeoutResult = context.GetTimeSpanOptionValue(command.PingTimeoutOption);

        if (!startTimeResult.IsSuccess)
        {
            toolWriter.WriteMessage(startTimeResult.Error);
            return;
        }

        if (pingTimeoutResult?.IsSuccess == false)
        {
            toolWriter.WriteMessage(pingTimeoutResult.Error);
            return;
        }

        var startTimerRequest = timerRequestFactory.CreateStartTimerRequest(user.Id, timerName, startTimeResult.Value, pingTimeoutResult?.Value);
        var startTimerResponse = await timerServiceApiClient.StartTimerAsync(startTimerRequest);

        if (startTimerResponse.IsSuccessStatusCode)
        {
            toolWriter.WriteMessage("Таймер успешно запущен");
            return;
        }

        toolWriter.WriteMessage(startTimerResponse.ResponseMessage ?? "Неизвестная ошибка");
    }
}