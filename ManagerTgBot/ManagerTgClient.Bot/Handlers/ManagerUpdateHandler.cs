using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace ManagerTgClient.Bot;

public class ManagerUpdateHandler: IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(update.Type.ToString());
            switch (update.Type)
            {
                case UpdateType.Message:
                {
                    var message = update.Message;
                    Console.WriteLine($"{update.Type}: {message.Text}");
                    await botClient.SendMessage(message.Chat.Id, "пошел нахуй");
                    break;
                }
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task HandleErrorAsync(
        ITelegramBotClient botClient,
        Exception exception,
        HandleErrorSource source,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}