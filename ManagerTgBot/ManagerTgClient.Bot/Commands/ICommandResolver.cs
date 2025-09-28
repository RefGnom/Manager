namespace Manager.ManagerTgClient.Bot.Commands;

public interface ICommandResolver
{
    IManagerBotCommand Resolve(string command);
}