using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands;

public interface ICommand
{
    string Name { get; }
    string Description { get; }
    Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest);
}