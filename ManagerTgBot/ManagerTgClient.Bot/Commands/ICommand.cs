using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Commands;

public interface ICommand
{
    Task<ICommandResult>  ExecuteAsync(ICommandRequest commandRequest);
    string Name { get; }
}