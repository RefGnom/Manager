using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Repository;

public interface ITelegramUserRepository
{
    Task CreateUserAsync(UserDto user);
    Task<UserDto?> FindAsync(long telegramId);
}