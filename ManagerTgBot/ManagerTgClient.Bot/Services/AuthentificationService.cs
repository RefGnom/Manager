using Manager.ManagerTgClient.Bot.Repository;
using Manager.ManagerTgClient.Bot.Repository.Model;
using Manager.ManagerTgClient.Bot.Services.Factories;

namespace Manager.ManagerTgClient.Bot.Services;

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

    public async Task<User?> FindUserAsync(long telegramId)
    {
        return await userRepository.FindAsync(telegramId);
    }
}