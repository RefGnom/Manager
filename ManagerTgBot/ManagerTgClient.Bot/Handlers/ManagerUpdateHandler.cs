using Manager.ManagerTgClient.Bot.Commands;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace ManagerTgClient.Bot;

public class ManagerUpdateHandler: IUpdateHandler
{
    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        try
        {
            Console.WriteLine(update.Type.ToString());
            switch (update.Message.Text)
            {
                case "/help":
                {
                    var command = new HelpCommand(botClient);
                    await command.ExecuteAsync(update.Message.Chat.Id);
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