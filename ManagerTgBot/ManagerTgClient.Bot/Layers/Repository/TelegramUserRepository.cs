using Manager.Core.EFCore;
using Manager.ManagerTgClient.Bot.Layers.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Layers.Repository;

public class TelegramUserRepository(IDataContext dataContext): ITelegramUserRepository
{
    public async Task CreateUserAsync(UserDbo userDbo) => await dataContext.InsertAsync(userDbo);
    public async Task<UserDbo?> FindAsync(long telegramId) => await dataContext.FindAsync<UserDbo, long>(telegramId);
}