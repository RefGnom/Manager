using Manager.Core.EFCore;
using Manager.ManagerTgClient.Bot.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Repository;

public class TelegramUserRepository(IDataContext dataContext): ITelegramUserRepository
{
    public Task CreateUserAsync(User user) => dataContext.InsertAsync(user);
    public Task<User?> FindAsync(long telegramId) => dataContext.FindAsync<User, long>(telegramId);
}