using System.Net;
using System.Threading.Tasks;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;
using ManagerService.Client;

namespace Manager.Tool.Layers.Logic.Timers;

public class DeleteTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter
) : CommandExecutorBase<DeleteTimerCommand>(toolCommandFactory)
{
    private readonly ITimerRequestFactory _timerRequestFactory = timerRequestFactory;
    private readonly ITimerServiceApiClient _timerServiceApiClient = timerServiceApiClient;
    private readonly IToolWriter _toolWriter = toolWriter;

    protected async override Task ExecuteAsync(CommandContext context, DeleteTimerCommand command)
    {
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;

        var deleteTimerRequest = _timerRequestFactory.CreateDeleteTimerRequest(context.EnsureUser().Id, timerName);
        var httpResponse = await _timerServiceApiClient.DeleteTimerAsync(deleteTimerRequest);

        if (httpResponse.IsSuccessStatusCode)
        {
            _toolWriter.WriteMessage("Таймер успешно удалён");
            return;
        }

        if (httpResponse.StatusCode is HttpStatusCode.NotFound)
        {
            _toolWriter.WriteMessage("Не нашли таймер с именем {0}", timerName);
            return;
        }

        _toolWriter.WriteMessage("Ошибка при удалении таймера: {0}", httpResponse.ResponseMessage);
    }
}