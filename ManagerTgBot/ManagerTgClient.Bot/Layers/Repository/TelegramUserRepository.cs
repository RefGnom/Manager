using Manager.Core.EFCore;
using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Repository;

public class TelegramUserRepository(IDataContext dataContext): ITelegramUserRepository
{
    public async Task CreateUserAsync(UserDto userDbo) => throw new NotImplementedException();
    public async Task<UserDto?> FindAsync(long telegramId) => throw new NotImplementedException();
}