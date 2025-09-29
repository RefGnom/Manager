using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Manager.ManagerTgClient.Bot;

public class ManagerBotRunner : IBotRunner
{
    public async Task RunAsync(string token)
    {
        using var cts = new CancellationTokenSource();
        var bot = new TelegramBotClient(token, cancellationToken: cts.Token);
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [],
        };
        var botHandler = new ManagerUpdateHandler();
        bot.StartReceiving(botHandler, receiverOptions, cancellationToken: cts.Token);
        await Task.Delay(-1, cts.Token);
    }
}