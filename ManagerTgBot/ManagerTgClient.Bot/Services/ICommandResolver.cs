namespace Manager.ManagerTgClient.Bot.Services;

public interface ICommandResolver
{
    ResolverData Resolve(string command);
}