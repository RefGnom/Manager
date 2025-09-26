using System.Reflection;
using Manager.ManagerTgClient.Bot.Commands.CommandsAttributes;

namespace Manager.ManagerTgClient.Bot.Commands;

public class CommandResolver : ICommandResolver
{
    private readonly Type[] commands = Assembly.GetEntryAssembly()!.GetTypes()
        .Where(type => typeof(IManagerBotCommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
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