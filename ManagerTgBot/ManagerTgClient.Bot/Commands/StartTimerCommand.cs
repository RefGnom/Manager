using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Commands;

public class StartTimerCommand : ICommand
{
    public async Task<ICommandResult> ExecuteAsync(ICommandRequest request) =>
        await Task.FromResult(new CommandResult("Таймер запущен"));

    public string Name => "/StartTimer";
}