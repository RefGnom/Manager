using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Results;
using Manager.ManagerTgClient.Bot.Layers.Services;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands;

public class CreateUserCommand(
    IAuthentificationService authentificationService
) : CommandBase
{
    public override string Name => "/createUser";
    public override string Description => "Создание нового пользователя";

    public override async Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest)
    {
        var request = CastRequest<CreateUserRequest>(commandRequest);
        await authentificationService.CreateUserAsync(request!.UserId, request.UserName);
        return await Task.FromResult(
            new CommandResult($"Пользователь с юзернеймом {request.UserName} успешно создан") as ICommandResult
        );
    }
}