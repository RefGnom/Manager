using Manager.ManagerTgClient.Bot.Commands;
using Manager.ManagerTgClient.Bot.Commands.Requests.Factories;

namespace Manager.ManagerTgClient.Bot.Services;

public class CommandResolver : ICommandResolver
{
    private readonly Dictionary<string, ResolverData> _resolverDataMap = new Dictionary<string, ResolverData>();

    public CommandResolver(IEnumerable<ICommand> commands, IEnumerable<ICommandRequestFactory> factories)
    {
        foreach (var command in commands)
        {
            _resolverDataMap[command.Name] = new ResolverData(
                command,
                factories.First(x => x.CommandName == command.Name)
            );
        }
    }

    public ResolverData Resolve(string userCommand)
    {
        var resolverData = _resolverDataMap[userCommand];
        return resolverData ?? throw new Exception("Unknown command: " + userCommand);
    }
}