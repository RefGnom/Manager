using System.Net;
using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class DeleteTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter,
    ILogger<DeleteTimerCommand> logger
) : CommandExecutorBase<DeleteTimerCommand>(toolCommandFactory, logger)
{
    protected override async Task ExecuteAsync(CommandContext context, DeleteTimerCommand command)
    {
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;

        var deleteTimerRequest = timerRequestFactory.CreateDeleteTimerRequest(context.EnsureUser().Id, timerName);
        var httpResponse = await timerServiceApiClient.DeleteTimerAsync(deleteTimerRequest);

        if (httpResponse.IsSuccessStatusCode)
        {
            toolWriter.WriteMessage("Таймер успешно удалён");
            return;
        }

        if (httpResponse.StatusCode is HttpStatusCode.NotFound)
        {
            toolWriter.WriteMessage("Не нашли таймер с именем {0}", timerName);
            return;
        }

        toolWriter.WriteMessage("Ошибка при удалении таймера: {0}", httpResponse.ResponseMessage);
    }
}