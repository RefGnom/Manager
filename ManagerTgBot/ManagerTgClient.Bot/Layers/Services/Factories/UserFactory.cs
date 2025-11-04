using Manager.ManagerTgClient.Bot.Layers.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Layers.Services.Factories;

public class UserFactory: IUserFactory
{
    public UserDbo CreateUser(long telegramId, string userName)
    {
        return new UserDbo(telegramId, Guid.NewGuid(), userName);
    }
}