using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States;

public class MainMenuState: IState
{
    public Task ProcessMessageAsync(Update update)
    {
        throw new NotImplementedException();
    }

    public Task ProcessCallbackAsync(Update update) => throw new NotImplementedException();

    public Task ProcessAsync(Update update) => throw new NotImplementedException();

    public Task InitializeAsync(Update update)
    {
        throw new NotImplementedException();
    }
}