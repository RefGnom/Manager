using Manager.Core.EFCore;
using Manager.ManagerTgClient.Bot.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Repository;

public class TelegramUserRepository(IDataContext dataContext): ITelegramUserRepository
{
    public async Task CreateUserAsync(User user) => await dataContext.InsertAsync(user);
    public async Task<User?> FindAsync(long telegramId) => await dataContext.FindAsync<User, long>(telegramId);
}