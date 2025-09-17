using System.Net;
using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class DeleteTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    ILogger<DeleteTimerCommand> logger
) : CommandExecutorBase<DeleteTimerCommand>(toolCommandFactory, logger)
{
    private readonly ILogger<DeleteTimerCommand> logger = logger;

    protected override async Task ExecuteAsync(CommandContext context, DeleteTimerCommand command)
    {
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;

        var deleteTimerRequest = timerRequestFactory.CreateDeleteTimerRequest(context.EnsureUser().Id, timerName);
        var httpResponse = await timerServiceApiClient.DeleteTimerAsync(deleteTimerRequest);

        if (httpResponse.IsSuccessStatusCode)
        {
            logger.WriteMessage("Таймер успешно удалён");
            return;
        }

        if (httpResponse.StatusCode is HttpStatusCode.NotFound)
        {
            logger.WriteMessage($"Не нашли таймер с именем {timerName}");
            return;
        }

        logger.WriteMessage($"Ошибка при удалении таймера: {httpResponse.ResponseMessage}");
    }
}