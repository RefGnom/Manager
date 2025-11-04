using Manager.ManagerTgClient.Bot.Layers.Repository;
using Manager.ManagerTgClient.Bot.Layers.Services.Factories;
using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Services;

public class AuthentificationService(ITelegramUserRepository userRepository, IUserFactory userFactory): IAuthentificationService
{
    public async Task CreateUserAsync(long telegramId, string userName)
    {
        // ToDO:
        //RecipientClient.CreateUser(userName)
        var user = userFactory.CreateUser(telegramId, userName);
        await userRepository.CreateUserAsync(user);
    }

    public async Task ConnectExistingUserAsync(long telegramId, string userName)
    {
        // ToDO:
        //globalUser = RecipientClient.Find(username)
        //telegramUser = new User(telegramId, user.Id, .....)
        var user = userFactory.CreateUser(telegramId, userName);
        await userRepository.CreateUserAsync(user);
    }

    public async Task<UserDto?> FindUserAsync(long telegramId)
    {
        return await userRepository.FindAsync(telegramId);
    }
}