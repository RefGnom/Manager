using System.Reflection;
using System.Text;
using Manager.ManagerTgClient.Bot.Commands.Attributes;
using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Commands;

[CommandName("/help")]
[CommandDescription("выводит подробную информацию по командам бота")]
public class HelpCommand : ICommand
{
    private readonly Type[] commands;

    public HelpCommand()
    {
        commands = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(type => typeof(ICommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            .ToArray();
    }

    public Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest)
    {
        var result = new StringBuilder();
        foreach (var command in commands)
        {
            var nameAttribute = command.GetCustomAttribute<CommandNameAttribute>();
            var descriptionAttribute = command.GetCustomAttribute<CommandDescriptionAttribute>();
            if (nameAttribute is not null && descriptionAttribute is not null)
            {
                result.Append($"{nameAttribute.Value} - {descriptionAttribute.Value}\n");
            }
        }

        return Task.FromResult<ICommandResult>(new CommandResult(result.ToString()));
    }

    public string Name => "/help";
}