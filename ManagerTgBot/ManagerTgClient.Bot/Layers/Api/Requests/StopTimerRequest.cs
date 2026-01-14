namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests;

public record StopTimerRequest(long UserId, string TimerName, DateTime StopTime);