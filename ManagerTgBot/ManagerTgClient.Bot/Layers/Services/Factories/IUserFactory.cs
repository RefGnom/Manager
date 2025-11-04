using Manager.ManagerTgClient.Bot.Layers.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Layers.Services.Factories;

public interface IUserFactory
{
    UserDbo CreateUser(long telegramId, string userName);
}