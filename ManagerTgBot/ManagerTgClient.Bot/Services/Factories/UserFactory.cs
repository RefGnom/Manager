using Manager.ManagerTgClient.Bot.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Services.Factories;

public class UserFactory: IUserFactory
{
    public User CreateUser(long telegramId, string userName)
    {
        return new User(telegramId, Guid.NewGuid(), userName);
    }
}