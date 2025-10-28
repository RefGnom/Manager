namespace Manager.ManagerTgClient.Bot.States;

public interface IStateManager
{
    IState GetState(long userId);
    void SetState(long userId, IState state);
}