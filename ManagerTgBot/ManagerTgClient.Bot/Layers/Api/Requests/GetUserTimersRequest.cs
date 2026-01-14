namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests;

public record GetUserTimersRequest(long UserId, bool WithArchived, bool WithDeleted);
