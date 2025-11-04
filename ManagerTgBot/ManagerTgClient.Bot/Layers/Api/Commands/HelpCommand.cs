using System.Text;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Results;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands;

public class HelpCommand(
    ICommand[] commands
) : CommandBase
{
    public override string Name => "/help";
    public override string Description => "выводит подробную информацию по командам бота";

    public override Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest)
    {
        var result = new StringBuilder();
        foreach (var command in commands)
        {
            var name = command.Name;
            var description = command.Description;
            result.Append($"{name} - {description}\n");
        }

        return Task.FromResult<ICommandResult>(new CommandResult(result.ToString()));
    }
}