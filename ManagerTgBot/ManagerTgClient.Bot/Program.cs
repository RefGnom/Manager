using Manager.ManagerTgClient.Bot.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.ManagerTgClient.Bot;

public class Program
{
    public static async Task Main(string[] args)
    {
        var token = ManagerBotConfigurator.GetToken();
        var serviceProvider = ApplicationConfigurator
            .CreateConfiguration()
            .CreateServiceProvider();
        var botRunner = serviceProvider.GetService<IBotRunner>();
        await botRunner.RunAsync(token);
    }
}