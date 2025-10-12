using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Commands;

public interface ICommand
{
    string Name { get; }
    Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest);
}