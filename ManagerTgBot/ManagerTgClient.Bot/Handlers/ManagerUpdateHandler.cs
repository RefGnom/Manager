using Manager.ManagerTgClient.Bot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Manager.ManagerTgClient.Bot.Handlers;

public class ManagerUpdateHandler(
    ICommandResolver commandResolver
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
            if (update.Type == UpdateType.Message)
            {
                var message = update.Message!.Text;
                var command = commandResolver.Resolve(message!);
                await command.ExecuteAsync(botClient, update.Message.Chat.Id);
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        HandleErrorSource source,
        CancellationToken cancellationToken
    ) => throw new NotImplementedException();
}