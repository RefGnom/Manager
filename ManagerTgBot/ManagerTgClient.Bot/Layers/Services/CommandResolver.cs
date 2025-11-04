using Manager.ManagerTgClient.Bot.Layers.Api.Commands;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Factories;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public class CommandResolver : ICommandResolver
{
    private readonly Dictionary<string, ResolverData> resolverDataMap = new();

    public CommandResolver(IEnumerable<ICommand> commands, IEnumerable<ICommandRequestFactory> factories)
    {
        foreach (var command in commands)
        {
            resolverDataMap[command.Name] = new ResolverData(
                command,
                factories.First(x => x.CommandName == command.Name)
            );
        }
    }

    public ResolverData Resolve(string userCommand)
    {
        var resolverData = resolverDataMap[userCommand];
        if (resolverData.Command is null || resolverData.Factory is null)
        {
            throw new ResolverMissingComponentException("No factory found for command: " + userCommand);
        }

        return resolverData;
    }
}