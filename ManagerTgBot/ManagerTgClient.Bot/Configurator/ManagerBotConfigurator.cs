using Microsoft.Extensions.Configuration;

namespace Manager.ManagerTgClient.Bot.Configurator;

public static class ManagerBotConfigurator
{
    public static string GetToken()
    {
        var configuration = new ConfigurationManager();
        configuration.AddUserSecrets<Program>()
            .Build();
        var token = configuration["ManagerTgBotToken"];
        return token ?? throw new InvalidOperationException("Bot token is missing");
    }
}