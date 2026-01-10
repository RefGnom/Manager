namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;

public record StartTimerRequest(
    long UserId,
    string Name,
    string Description
) : ICommandRequest;