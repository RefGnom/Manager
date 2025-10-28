using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States;

public interface IState
{
    Task ProcessUpdateAsync(Update update);
    Task InitializeAsync(Update update);
}