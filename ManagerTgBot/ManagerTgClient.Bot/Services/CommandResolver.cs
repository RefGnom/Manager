using System.Reflection;
using Manager.ManagerTgClient.Bot.Commands;
using Manager.ManagerTgClient.Bot.Commands.Attributes;

namespace Manager.ManagerTgClient.Bot.Services;

public class CommandResolver : ICommandResolver
{
    private readonly ResolverData[] commands = Assembly.GetEntryAssembly()!.GetTypes()
        .Where(type =>
            typeof(ICommand).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false }
        ).Select(commandType => new ResolverData(commandType, commandType.GetGenericArguments().First()))
        .ToArray();

    public ResolverData Resolve(string userCommand)
    {
        var result =
            commands.First(x => $"{x.CommandType.GetCustomAttribute<CommandNameAttribute>()!.Value}".Equals(userCommand)
            );
        if (result is null)
        {
            throw new ArgumentException($"{userCommand} is not a valid command");
        }

        return result;
    }
}