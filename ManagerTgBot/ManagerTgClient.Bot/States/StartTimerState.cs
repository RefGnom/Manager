using Manager.ManagerTgClient.Bot.Commands.Requests.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States;

public class EnteringTimerNameState(
    ITelegramBotClient botClient,
    IStartTimerRequestBuilder builder
) : IState
{
    public Task ProcessAsync(Update update) => throw new NotImplementedException();

    public async Task InitializeAsync(Update update)
    {
        builder.Initialize();
        await botClient.SendMessage(update.Message!.Chat.Id, "Введите название своего таймера");
    }
}