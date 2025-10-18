using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;
using Manager.ManagerTgClient.Bot.Services;

namespace Manager.ManagerTgClient.Bot.Commands;

public class CreateUserCommand(IAuthentificationService authentificationService) : ICommand
{
    public string Name => "/createUser";

    public async Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest)
    {
        await authentificationService.CreateUserAsync()
    }

}