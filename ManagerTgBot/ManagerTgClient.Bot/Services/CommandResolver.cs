using System.Reflection;
using Manager.ManagerTgClient.Bot.Commands;
using Manager.ManagerTgClient.Bot.Commands.Attributes;

namespace Manager.ManagerTgClient.Bot.Services;

public class CommandResolver : ICommandResolver
{
    private readonly Type[] commands = Assembly.GetEntryAssembly()!.GetTypes()
        .Where(type => typeof(IManagerBotCommand).IsAssignableFrom(type) && type is { IsClass: true, IsAbstract: false })
        .ToArray();

    public IManagerBotCommand Resolve(string userCommand)
    {
        var resultType =
            commands.FirstOrDefault(x => $"{x.GetCustomAttribute<CommandNameAttribute>()!.Value}".Equals(userCommand));
        if (resultType is null)
        {
            throw new ArgumentException($"{userCommand} is not a valid command");
        }

        var command = (IManagerBotCommand)Activator.CreateInstance(resultType)!;
        return command;
    }
}