using Manager.ManagerTgClient.Bot.Layers.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Layers.Repository;

public interface ITelegramUserRepository
{
    Task CreateUserAsync(UserDbo userDbo);
    Task<UserDbo?> FindAsync(long telegramId);
}