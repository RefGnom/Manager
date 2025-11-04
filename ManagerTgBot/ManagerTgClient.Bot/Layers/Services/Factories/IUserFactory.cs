using Manager.ManagerTgClient.Bot.Layers.Services.Models;

namespace Manager.ManagerTgClient.Bot.Layers.Services.Factories;

public interface IUserFactory
{
    UserDto CreateUser(long telegramId, string userName);
}