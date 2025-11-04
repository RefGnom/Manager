namespace Manager.ManagerTgClient.Bot.Layers.Services;

public class ResolverMissingComponentException(
    string message
) : Exception(message) { }