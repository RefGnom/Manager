using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Services.Factories;

public class UserFactory : IUserFactory
{
    public UserDto CreateUser(long telegramId, string userName) => new(telegramId, Guid.NewGuid(), userName);
}