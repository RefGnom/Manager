namespace Manager.ManagerTgClient.Bot.Application.Configuration;

public class ConfigurationMissingTokenException(
    string message
) : Exception(message);