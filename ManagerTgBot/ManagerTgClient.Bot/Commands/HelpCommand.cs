using Manager.ManagerTgClient.Bot.Commands.Attributes;
using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;


namespace Manager.ManagerTgClient.Bot.Commands;

[CommandName("/help")]
[CommandDescription("выводит подробную информацию по командам бота")]
public class HelpCommand: ICommand
{
    public Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest) => throw new NotImplementedException();

    public string Name => "/help";
}