using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States;

public interface IState
{
    void Process(Update update);
}