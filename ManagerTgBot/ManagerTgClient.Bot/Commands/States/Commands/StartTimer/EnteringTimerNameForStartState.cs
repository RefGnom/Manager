using Manager.ManagerTgClient.Bot.Commands.Requests.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Commands.States.Commands.StartTimer;

public class EnteringTimerNameForStartState(
    ITelegramBotClient botClient,
    IStartTimerRequestBuilder builder
) : IState
{
    public Task ProcessUpdateAsync(Update update) => throw new NotImplementedException();

    public async Task InitializeAsync(long userId)
    {
        builder = new StartTimerRequestBuilder();
        await botClient.SendMessage(userId, "Введите название своего таймера");
    }
}