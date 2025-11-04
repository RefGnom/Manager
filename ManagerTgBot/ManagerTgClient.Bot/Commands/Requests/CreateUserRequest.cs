namespace Manager.ManagerTgClient.Bot.Commands.Requests;

public record class CreateUserRequest(
    string UserName,
    long UserId
) : ICommandRequest;