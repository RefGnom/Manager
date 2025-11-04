using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public interface IState
{
    Task ProcessUpdateAsync(Update update);
    Task InitializeAsync(long userId);
}