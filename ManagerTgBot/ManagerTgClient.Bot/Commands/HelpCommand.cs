using System.Reflection;
using System.Text;
using Manager.ManagerTgClient.Bot.Commands.CommandsAttributes;
using Telegram.Bot;

namespace Manager.ManagerTgClient.Bot.Commands;

[CommandName("/help")]
[CommandDescription("выводит подробную информацию по командам бота")]
public class HelpCommand(
    ITelegramBotClient botClient
) : IManagerBotCommand
{
    private readonly Type[] commands = Assembly.GetEntryAssembly()!.GetExportedTypes();

    public async Task ExecuteAsync(long chatId)
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