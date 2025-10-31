using Manager.ManagerTgClient.Bot.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Repository;

public interface ITelegramUserRepository
{
    Task CreateUserAsync(User user);
    Task<User?> FindAsync(long telegramId);
}