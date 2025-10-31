using Manager.ManagerTgClient.Bot.Commands.States;
using Manager.ManagerTgClient.Bot.Extentions;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Handlers;

public class ManagerUpdateHandler(
    IStateManager stateManager
) : IManagerUpdateHandler
{
    public async Task HandleUpdateAsync(
        ITelegramBotClient botClient,
        Update update,
        CancellationToken cancellationToken
    )
    {
        try
        {
            var chatId = update.GetChatId();
            var state = stateManager.GetState(chatId);
            await state.ProcessUpdateAsync(update);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }

    public Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        HandleErrorSource source,
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();
}