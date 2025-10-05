using Manager.ManagerTgClient.Bot.Commands;

namespace Manager.ManagerTgClient.Bot.Services;

public interface ICommandResolver
{
    IManagerBotCommand Resolve(string command);
}