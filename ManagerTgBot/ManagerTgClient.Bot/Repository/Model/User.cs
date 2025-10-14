namespace Manager.ManagerTgClient.Bot.Repository.Model;

public class User()
{
    public required long TelegramId { get; init; }
    public required Guid ServerId { get; init; }
    public required string Username { get; init; }
}