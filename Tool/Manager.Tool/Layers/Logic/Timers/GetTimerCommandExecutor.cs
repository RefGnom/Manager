using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Extensions;
using Manager.Core.Extensions.LinqExtensions;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;

namespace Manager.Tool.Layers.Logic.Timers;

public class GetTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter
) : CommandExecutorBase<GetTimerCommand>(toolCommandFactory)
{
    private readonly ITimerRequestFactory _timerRequestFactory = timerRequestFactory;
    private readonly ITimerServiceApiClient _timerServiceApiClient = timerServiceApiClient;
    private readonly IToolWriter _toolWriter = toolWriter;

    protected async override Task ExecuteAsync(CommandContext context, GetTimerCommand command)
    {
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;

        var getTimerRequest = _timerRequestFactory.CreateTimerRequest(context.EnsureUser().Id, timerName);
        var timerResponse = await _timerServiceApiClient.FindTimerAsync(getTimerRequest);

        if (timerResponse is null)
        {
            _toolWriter.WriteMessage("Не нашли таймер с именем {0}", timerName);
            return;
        }

        _toolWriter.WriteMessage(
            """
            Имя таймера "{0}"
            Запущен в {1}
            Прошло {2}
            Уведомление в {3}
            Сессии
            {4}
            Находится в статусе {5}
            """,
            timerResponse.Name,
            timerResponse.StartTime,
            timerResponse.ElapsedTime,
            timerResponse.StartTime + timerResponse.PingTimeout,
            timerResponse.Sessions.Select(FormatTimerSession).JoinToString('\n'),
            timerResponse.TimerStatus.GetDescription()
        );
        return;

        string FormatTimerSession(TimerSessionResponse session)
        {
            var sessionStatus = session.StopTime is null ? "активна" : $"закончилась в {session.StopTime}";
            return $"  Началась в {session.StartTime}, {sessionStatus}";
        }
    }
}