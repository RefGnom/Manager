namespace Manager.ManagerTgClient.Bot.Layers.Services;

public interface ICommandResolver
{
    ResolverData Resolve(string command);
}