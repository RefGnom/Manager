namespace Manager.ManagerTgClient.Bot.Configuration;

public class ConfigurationMissingTokenException(
    string message
) : Exception(message);