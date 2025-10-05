using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Commands;

public interface IManagerBotCommand<T> where T : IManagerCommandRequest
{
    Task<ICommandResult> ExecuteAsync(T commandRequest);
}