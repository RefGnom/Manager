using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Commands;

public class StartTimerCommand : IManagerBotCommand<StartTimerRequest>
{
    public async Task<ICommandResult> ExecuteAsync(StartTimerRequest commandRequest)
    {
        return await Task.FromResult(new CommandResult("Таймер запущен"));
    }
}