using System.Net;
using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class ResetTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    ILogger<ResetTimerCommand> logger
) : CommandExecutorBase<ResetTimerCommand>(toolCommandFactory, logger)
{
    private readonly ILogger<ResetTimerCommand> logger = logger;

    protected override async Task ExecuteAsync(CommandContext context, ResetTimerCommand command)
    {
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;

        var deleteTimerRequest = timerRequestFactory.CreateCommonTimerRequest(context.EnsureUser().Id, timerName);
        var httpResponse = await timerServiceApiClient.DeleteTimerAsync(deleteTimerRequest);

        if (httpResponse.IsSuccessStatusCode)
        {
            logger.WriteMessage("Таймер успешно сброшен");
            return;
        }

        if (httpResponse.StatusCode is HttpStatusCode.NotFound)
        {
            logger.WriteMessage($"Не нашли таймер с именем {timerName}");
            return;
        }

        logger.WriteMessage($"Ошибка при сбросе таймера: {httpResponse.ResponseMessage}");
    }
}