namespace Manager.ManagerTgClient.Bot.Commands.Requests;

public record StartTimerRequest(
    long UserId,
    string TimerName
) : ICommandRequest;
