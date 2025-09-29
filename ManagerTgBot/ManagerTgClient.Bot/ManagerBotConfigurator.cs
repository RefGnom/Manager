using Microsoft.Extensions.Configuration;

namespace Manager.ManagerTgClient.Bot;

public static class ManagerBotConfigurator
{
    public static async Task<string> GetTokenAsync()
    {
        var configuration = new ConfigurationManager();
        configuration.AddUserSecrets<Program>()
            .Build();
        var token = configuration["ManagerTgBotToken"];
        if (token is null)
        {
            throw new InvalidOperationException("Bot token is missing");
        }

        return token;
    }
}