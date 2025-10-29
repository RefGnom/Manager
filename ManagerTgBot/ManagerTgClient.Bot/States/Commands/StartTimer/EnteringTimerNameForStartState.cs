using Manager.ManagerTgClient.Bot.Commands.Requests.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States.Commands.StartTimer;

public class EnteringTimerNameForStartState(
    ITelegramBotClient botClient,
    IStartTimerRequestBuilder builder
) : IState
{
    public Task ProcessUpdateAsync(Update update) => throw new NotImplementedException();

    public async Task InitializeAsync(long chatId)
    {
        builder.Initialize();
        await botClient.SendMessage(chatId, "Введите название своего таймера");
    }
}