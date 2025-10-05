using System.Reflection;
using System.Text;
using Manager.ManagerTgClient.Bot.Commands.CommandResults;
using Manager.ManagerTgClient.Bot.Commands.CommandsAttributes;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Commands;

[CommandName("/help")]
[CommandDescription("выводит подробную информацию по командам бота")]
public class HelpCommand : IManagerBotCommand
{
    private readonly Type[] commands;

    public HelpCommand()
    {
        commands = Assembly.GetEntryAssembly()!.GetTypes()
            .Where(type => typeof(IManagerBotCommand).IsAssignableFrom(type) && type.IsClass && !type.IsAbstract)
            .ToArray();
    }

    public async Task<ICommandResult> ExecuteAsync(ITelegramBotClient botClient, long chatId)
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
        return await Task.FromResult(new CommandResult(result.ToString()));
    }
}