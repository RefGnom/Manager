using Manager.ManagerTgClient.Bot.Commands;
using Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

namespace Manager.ManagerTgClient.Bot.Services;

public class ResolverData(ICommand command, ICommandRequestFactory factory)
{
    public ICommand Command = command;
    public ICommandRequestFactory Factory = factory;
}