using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;
using Manager.ManagerTgClient.Bot.Services;

namespace Manager.ManagerTgClient.Bot.Commands;

public class CreateUserCommand(
    IAuthentificationService authentificationService
) : CommandBase
{
    public override string Name => "/createUser";
    public override string Description => "Создание нового пользователя";

    public override async Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest)
    {
        var request = CastRequest<CreateUserRequest>(commandRequest);
        await authentificationService.CreateUserAsync(request!.TelegramId, request.UserName);
        return await Task.FromResult(
            new CommandResult($"Пользователь с юзернеймом {request.UserName} успешно создан") as ICommandResult
        );
    }
}