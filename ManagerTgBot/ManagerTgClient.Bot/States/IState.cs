using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States;

public interface IState
{
    Task ProcessAsync(Update update);
    Task InitializeAsync(Update update);
}