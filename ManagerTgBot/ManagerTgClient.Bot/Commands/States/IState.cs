using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Commands.States;

public interface IState
{
    Task ProcessUpdateAsync(Update update);
    Task InitializeAsync(long chatId);
}