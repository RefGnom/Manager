namespace Manager.ManagerTgClient.Bot.Commands.Results;

public record CommandResult(
    string Message
) : ICommandResult { }