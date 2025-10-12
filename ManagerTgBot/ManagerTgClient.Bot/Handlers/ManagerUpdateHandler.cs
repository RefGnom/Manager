using Manager.ManagerTgClient.Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Manager.ManagerTgClient.Bot.Handlers;

public class ManagerUpdateHandler(
    ICommandExecutor commandExecutor
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
            if (update is { Type: UpdateType.Message, Message.Text: not null })
            {
                await commandExecutor.ExecuteAsync(update.Message.Text, update.Message.Chat.Id);
            }
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