using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States;

public interface IState
{
    Task ProcessUpdateAsync(Update update);
    Task InitializeAsync(long userId);
}