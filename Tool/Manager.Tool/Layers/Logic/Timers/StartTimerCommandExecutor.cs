using System.Threading.Tasks;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;

namespace Manager.Tool.Layers.Logic.Timers;

public class StartTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter,
    IUserTimeService userTimeService
) : CommandExecutorBase<StartTimerCommand>(toolCommandFactory)
{
    private readonly ITimerRequestFactory _timerRequestFactory = timerRequestFactory;
    private readonly ITimerServiceApiClient _timerServiceApiClient = timerServiceApiClient;
    private readonly IToolWriter _toolWriter = toolWriter;
    private readonly IUserTimeService _userTimeService = userTimeService;

    protected async override Task ExecuteAsync(CommandContext context, StartTimerCommand command)
    {
        var user = context.EnsureUser();
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;
        var startTimeResult = context.GetDateTimeOptionValue(command.StartTimeOption) ?? _userTimeService.GetUserTime(user);
        var pingTimeoutResult = context.GetTimeSpanOptionValue(command.PingTimeoutOption);

        if (!startTimeResult.IsSuccess)
        {
            _toolWriter.WriteMessage(startTimeResult.FailureMessage);
            return;
        }

        if (pingTimeoutResult?.IsSuccess == false)
        {
            _toolWriter.WriteMessage(pingTimeoutResult.FailureMessage);
            return;
        }

        var startTimerRequest = _timerRequestFactory.CreateStartTimerRequest(user.Id, timerName, startTimeResult.Value, pingTimeoutResult?.Value);
        var startTimerResponse = await _timerServiceApiClient.StartTimerAsync(startTimerRequest);

        if (startTimerResponse.IsSuccessStatusCode)
        {
            _toolWriter.WriteMessage("Таймер успешно запущен");
            return;
        }

        _toolWriter.WriteMessage(startTimerResponse.ResponseMessage ?? "Неизвестная ошибка");
    }
}