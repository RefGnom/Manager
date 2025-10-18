using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;
using Manager.TimerService.Client;

namespace Manager.ManagerTgClient.Bot.Commands;

public class StartTimerCommand(ITimerServiceApiClient timerApi) : ICommand
{
    public async Task<ICommandResult> ExecuteAsync(ICommandRequest request)
    {
        throw new NotImplementedException();
    }

    public string Name => "/StartTimer";
}