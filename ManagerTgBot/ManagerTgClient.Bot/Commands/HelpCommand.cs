using System.Reflection;
using System.Text;
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

    public async Task ExecuteAsync(ITelegramBotClient botClient, long chatId)
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

        await botClient.SendMessage(chatId, result.ToString());
    }
}