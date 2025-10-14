namespace Manager.ManagerTgClient.Bot.Repository.Model;

public class User()
{
    public required Guid TelegramId { get; init; }
    public required Guid ServerId { get; init; }
    public required string Username { get; init; }
}