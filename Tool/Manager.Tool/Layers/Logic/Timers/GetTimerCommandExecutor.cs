using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Common.Enum;
using Manager.Core.Common.Linq;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels;
using Manager.Tool.Layers.Logic.CommandsCore;
using Manager.Tool.Layers.Presentation;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class GetTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    IToolWriter toolWriter,
    ILogger<GetTimerCommand> logger
) : CommandExecutorBase<GetTimerCommand>(toolCommandFactory, logger)
{
    protected override async Task ExecuteAsync(CommandContext context, GetTimerCommand command)
    {
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;

        var getTimerRequest = timerRequestFactory.CreateTimerRequest(context.EnsureUser().Id, timerName);
        var timerResponse = await timerServiceApiClient.FindTimerAsync(getTimerRequest);

        if (timerResponse is null)
        {
            toolWriter.WriteMessage("Не нашли таймер с именем {0}", timerName);
            return;
        }

        toolWriter.WriteMessage(
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