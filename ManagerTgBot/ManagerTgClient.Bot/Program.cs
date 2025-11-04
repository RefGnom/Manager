using Manager.ManagerTgClient.Bot.Application;
using Manager.ManagerTgClient.Bot.Application.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.ManagerTgClient.Bot;

public class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = ApplicationConfigurator
            .CreateConfiguration()
            .CreateServiceProvider();

        var botRunner = serviceProvider.GetRequiredService<IBotRunner>();
        await botRunner.RunAsync();
    }
}