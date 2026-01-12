namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests;

public record StartTimerRequest(
    long UserId,
    string Name,
    DateTime StartTime,
    TimeSpan? PingTimeout
);