using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;

namespace Manager.Tool.Layers.Logic.Timers;

public class StopTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter,
    IUserTimeService userTimeService
) : CommandExecutorBase<StopTimerCommand>(toolCommandFactory)
{
    private readonly ITimerRequestFactory _timerRequestFactory = timerRequestFactory;
    private readonly ITimerServiceApiClient _timerServiceApiClient = timerServiceApiClient;
    private readonly IToolWriter _toolWriter = toolWriter;
    private readonly IUserTimeService _userTimeService = userTimeService;

    protected async override Task ExecuteAsync(CommandContext context, StopTimerCommand command)
    {
        var user = context.EnsureUser();
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;
        var stopTimeResult = context.GetDateTimeOptionValue(command.StopTimeOption) ?? _userTimeService.GetUserTime(user);

        if (!stopTimeResult.IsSuccess)
        {
            _toolWriter.WriteMessage(stopTimeResult.FailureMessage);
            return;
        }

        var stopTimerRequest = _timerRequestFactory.CreateStopTimerRequest(user.Id, timerName, stopTimeResult.Value);
        var stopTimerResponse = await _timerServiceApiClient.StopTimerAsync(stopTimerRequest);

        if (stopTimerResponse.IsSuccessStatusCode)
        {
            _toolWriter.WriteMessage("Таймер успешно остановлен");
            return;
        }

        _toolWriter.WriteMessage(stopTimerResponse.ResponseMessage ?? "Неизвестная ошибка");
    }
}