using Manager.ManagerTgClient.Bot.Handlers;
using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Manager.ManagerTgClient.Bot;

public class Program
{
    public static async Task Main(string[] args)
    {
        using var cts = new CancellationTokenSource
        (
        );

        var configuration = new ConfigurationManager();
        configuration.AddUserSecrets<Program>()

            .Build();
        var token = configuration["ManagerTgBotToken"];
        Console.WriteLine(
            token
        );

        var bot = new TelegramBotClient(configuration["ManagerTgBotToken"]!, cancellationToken: cts.Token);
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = [],
        };
        var botHandler = new ManagerUpdateHandler();
        bot.StartReceiving(botHandler, receiverOptions, cts.Token);
        await Task.Delay(-1, cts.Token);
    }
}