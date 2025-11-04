using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Repository;

public class TelegramUserRepository : ITelegramUserRepository
{
    public Task CreateUserAsync(UserDto user) => throw new NotImplementedException();
    public Task<UserDto?> FindAsync(long telegramId) => throw new NotImplementedException();
}