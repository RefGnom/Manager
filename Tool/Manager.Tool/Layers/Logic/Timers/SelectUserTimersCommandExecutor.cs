using System.Threading.Tasks;
using Manager.Core.Extensions;
using Manager.TimerService.Client;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;

namespace Manager.Tool.Layers.Logic.Timers;

public class SelectUserTimersCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter
) : CommandExecutorBase<SelectUserTimersCommand>(toolCommandFactory)
{
    private readonly ITimerRequestFactory _timerRequestFactory = timerRequestFactory;
    private readonly ITimerServiceApiClient _timerServiceApiClient = timerServiceApiClient;
    private readonly IToolWriter _toolWriter = toolWriter;

    protected async override Task ExecuteAsync(CommandContext context, SelectUserTimersCommand command)
    {
        var withDeleted = context.ContainsOption("--with_deleted");
        var withArchived = context.ContainsOption("--with_archived");

        var userTimersRequest = _timerRequestFactory.CreateUserTimersRequest(context.EnsureUser().Id, withDeleted, withArchived);
        var userTimersResponse = await _timerServiceApiClient.SelectUserTimersAsync(userTimersRequest);

        if (userTimersResponse.Timers.Length == 0)
        {
            _toolWriter.WriteMessage("У вас нет таймеров");
            return;
        }

        var timers = userTimersResponse.Timers;
        for (var i = 0; i < timers.Length; i++)
        {
            var timer = timers[i];
            _toolWriter.WriteMessage("{0}. {1}, {2}", NormalizeTimerNumber(i + 1, timers.Length), timer.Name, timer.TimerStatus.GetDescription());
        }
    }

    private string NormalizeTimerNumber(int number, int timersCount)
    {
        var numberLength = timersCount.ToString().Length;
        return number.ToString().PadLeft(numberLength);
    }
}