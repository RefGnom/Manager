using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Common.Enum;
using Manager.Core.Common.Linq;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels;
using Manager.Tool.Layers.Logic.CommandsCore;
using Microsoft.Extensions.Logging;

namespace Manager.Tool.Layers.Logic.Timers;

public class GetTimerCommandExecutor(
    IToolCommandFactory toolCommandFactory,
    ITimerRequestFactory timerRequestFactory,
    ITimerServiceApiClient timerServiceApiClient,
    ILogger<GetTimerCommand> logger
) : CommandExecutorBase<GetTimerCommand>(toolCommandFactory, logger)
{
    private readonly ILogger<GetTimerCommand> logger = logger;

    protected override async Task ExecuteAsync(CommandContext context, GetTimerCommand command)
    {
        var timerName = context.GetCommandArgument(command.CommandName) ?? TimerCommandConstants.DefaultTimerName;

        var getTimerRequest = timerRequestFactory.CreateTimerRequest(context.EnsureUser().Id, timerName);
        var timerResponse = await timerServiceApiClient.FindTimerAsync(getTimerRequest);

        if (timerResponse is null)
        {
            logger.WriteMessage($"Не нашли таймер с именем {timerName}");
            return;
        }

        logger.WriteMessage(
            $"""
             Имя таймера "{timerResponse.Name}"
             Запущен в {timerResponse.StartTime}
             Прошло {timerResponse.ElapsedTime}
             Уведомление в {timerResponse.StartTime + timerResponse.PingTimeout}
             Сессии
             {timerResponse.Sessions.Select(FormatTimerSession).JoinToString('\n')}
             Находится в статусе {timerResponse.TimerStatus.GetDescription()}
             """
        );
        return;

        string FormatTimerSession(TimerSessionResponse session)
        {
            var sessionStatus = session.StopTime is null ? "активна" : $"закончилась в {session.StopTime}";
            return $"  Началась в {session.StartTime}, {sessionStatus}";
        }
    }
}