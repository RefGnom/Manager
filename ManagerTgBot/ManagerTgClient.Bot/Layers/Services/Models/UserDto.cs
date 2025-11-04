namespace Manager.ManagerTgClient.Bot.Layers.Services.Models;

public record UserDto(
    long UserId,
    Guid RecipientId,
    string UserName
);