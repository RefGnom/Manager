namespace Manager.ManagerTgClient.Bot.Services;

public interface ICommandResolver
{
    Task<ResolverData> ResolveAsync(string command);
}