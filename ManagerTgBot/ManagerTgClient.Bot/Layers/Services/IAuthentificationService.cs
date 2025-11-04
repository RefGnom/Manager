using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public interface IAuthentificationService
{
    Task CreateUserAsync(long telegramId, string userName);
    Task ConnectExistingUserAsync(long telegramId, string userName);
    Task<UserDto?> FindUserAsync(long telegramId);
}