namespace Manager.ManagerTgClient.Bot.Configurator;

public class ConfigurationMissingTokenException(
    string message
) : Exception(message);