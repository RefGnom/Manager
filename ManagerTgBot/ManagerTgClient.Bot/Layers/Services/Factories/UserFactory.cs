using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Services.Factories;

public class UserFactory: IUserFactory
{
    public UserDto CreateUser(long telegramId, string userName)
    {
        return new UserDto(telegramId, Guid.NewGuid(), userName);
    }
}