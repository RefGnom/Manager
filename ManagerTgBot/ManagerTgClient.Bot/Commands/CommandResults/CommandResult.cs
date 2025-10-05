namespace Manager.ManagerTgClient.Bot.Commands.CommandResults;

public class CommandResult(
    string value
) : ICommandResult
{
    public string Value { get; init; } = value;
}