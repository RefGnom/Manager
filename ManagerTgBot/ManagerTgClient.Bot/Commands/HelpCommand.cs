using Manager.ManagerTgClient.Bot.Commands.CommandsAttributes;

namespace Manager.ManagerTgClient.Bot.Commands;

[CommandName("/help")]
[CommandDescription("выводит подробную информацию по командам бота")]
public class HelpCommand: IManagerBotCommand
{
    public Task ExecuteAsync(long chatId)
    {
        throw new NotImplementedException();
    }
}