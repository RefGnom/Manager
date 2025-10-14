namespace Manager.ManagerTgClient.Bot.Repository.Model;

public record User(Guid TelegramId, Guid ServerId, string UserName)
{
}