using Manager.ManagerTgClient.Bot.Commands.Attributes;
using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;
using Manager.ManagerTgClient.Bot.Services;

namespace Manager.ManagerTgClient.Bot.Commands;

[CommandName("/createUser")]
[CommandDescription("выводит подробную информацию по командам бота")]
public class CreateUserCommand(
    IAuthentificationService authentificationService
) : ICommand
{
    public string Name => "/createUser";

    public async Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest)
    {
        var request = commandRequest as CreateUserRequest;
        await authentificationService.CreateUserAsync(request!.TelegramId, request.UserName);
        return await Task.FromResult(
            new CommandResult($"Пользователь с юзернеймом {request.UserName} успешно создан") as ICommandResult
        );
    }
}