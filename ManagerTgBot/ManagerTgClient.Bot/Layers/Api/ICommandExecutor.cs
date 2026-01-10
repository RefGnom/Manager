using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;

namespace Manager.ManagerTgClient.Bot.Layers.Api;

public interface ICommandExecutor
{
    Task SendMessageAsync(long userId, string message);
    Task StartTimerAsync(StartTimerRequest request);
}