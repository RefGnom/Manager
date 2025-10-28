using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States;

public class TimerMenuState: IState
{
    public Task ProcessUpdateAsync(Update update) => throw new NotImplementedException();

    public Task InitializeAsync(long chatId) => throw new NotImplementedException();
}