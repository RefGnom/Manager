using Manager.ManagerTgClient.Bot.Repository.Model;

namespace Manager.ManagerTgClient.Bot.Services.Factories;

public interface IUserFactory
{
    User CreateUser(long telegramId, string userName);
}