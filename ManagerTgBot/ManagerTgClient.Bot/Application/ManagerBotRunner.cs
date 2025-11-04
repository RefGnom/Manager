using Manager.ManagerTgClient.Bot.Application.Handlers;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Application;

public class ManagerBotRunner(
    ITelegramBotClient botClient,
    IManagerUpdateHandler botHandler
) : IBotRunner
{
    public async Task RunAsync()
    {
        using var cts = new CancellationTokenSource();

        botClient.StartReceiving(botHandler, cancellationToken: cts.Token);
        await Task.Delay(-1, cts.Token);
    }
}