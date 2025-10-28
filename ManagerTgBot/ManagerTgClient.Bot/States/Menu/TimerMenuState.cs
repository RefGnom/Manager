using Telegram.Bot;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public class TimerMenuState(ITelegramBotClient botClient) : IState
{
    public Task ProcessUpdateAsync(Update update)
    {
        throw new NotImplementedException();
    }

    public Task InitializeAsync(long chatId)
    {
        botClient.SendMessage(chatId, "Выберите выполняемое действие");
        return Task.CompletedTask;
    }
}