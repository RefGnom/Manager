namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;

public record class CreateUserRequest(
    string UserName,
    long UserId
) : ICommandRequest;