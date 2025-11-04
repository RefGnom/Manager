namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Results;

public record CommandResult(
    string Message
) : ICommandResult { }