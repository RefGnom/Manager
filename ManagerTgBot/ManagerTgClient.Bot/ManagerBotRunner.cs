using Manager.ManagerTgClient.Bot.Configuration;
using Manager.ManagerTgClient.Bot.Handlers;
using Microsoft.Extensions.Options;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot;

public class ManagerBotRunner(
    IManagerUpdateHandler botHandler,
    IOptions<ManagerBotOptions> managerBotOptions
) : IBotRunner
{
    public async Task RunAsync()
    {
        using var cts = new CancellationTokenSource();

        var botClient = new TelegramBotClient(managerBotOptions.Value.ManagerTgBotToken, cancellationToken: cts.Token);
        botClient.StartReceiving(botHandler, cancellationToken: cts.Token);
        await Task.Delay(-1, cts.Token);
    }
}