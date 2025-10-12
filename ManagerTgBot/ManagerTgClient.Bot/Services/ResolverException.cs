namespace Manager.ManagerTgClient.Bot.Services;

public class ResolverMissingComponentException(
    string message
) : Exception(message) { }