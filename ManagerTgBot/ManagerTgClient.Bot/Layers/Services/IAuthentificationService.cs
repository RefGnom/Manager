using Manager.ManagerTgClient.Bot.Layers.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public interface IAuthentificationService
{
    Task CreateUserAsync(long telegramId, string userName);
    Task ConnectExistingUserAsync(long telegramId, string userName);
    Task<UserDbo?> FindUserAsync(long telegramId);
}