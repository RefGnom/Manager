namespace Manager.ManagerTgClient.Bot.Commands.Results;

public class CommandResult(
    string value
) : ICommandResult
{
    public string Value { get; init; } = value;
}