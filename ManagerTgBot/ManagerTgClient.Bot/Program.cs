using Microsoft.Extensions.Configuration;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Manager.ManagerTgClient.Bot;

class Program
{
    public async static Task Main(string[] args)
    {
        using var cts = new CancellationTokenSource();

        var configuration = new ConfigurationManager();
        configuration.AddUserSecrets<Program>()
            .Build();
        var token = configuration["ManagerTgBotToken"];
        Console.WriteLine(token);

        var bot = new TelegramBotClient(configuration["ManagerTgBotToken"], cancellationToken: cts.Token);
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = Array.Empty<UpdateType>(),
        };
        var botHandler = new ManagerUpdateHandler();
        bot.StartReceiving(botHandler, receiverOptions, cancellationToken: cts.Token);
        await Task.Delay(-1);
    }
}