using Manager.ManagerTgClient.Bot.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Services;

public interface IAuthentificationService
{
    Task CreateUserAsync(long telegramId, string userName);
    Task ConnectExistingUserAsync(long telegramId, string userName);
    Task<User?> FindUserAsync(long telegramId);
}