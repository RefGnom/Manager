using Manager.ManagerTgClient.Bot.Repository;
using Manager.ManagerTgClient.Bot.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Services;

public class AuthentificationService(ITelegramUserRepository userRepository): IAuthentificationService
{
    public async Task CreateUserAsync(long telegramId, string userName)
    {
        await userRepository.CreateUserAsync()
    }

    public async Task ConnectExistingUserAsync(string userName) => throw new NotImplementedException();

    public async Task<User?> AuthenticateUserAsync(long telegramId) => throw new NotImplementedException();
}