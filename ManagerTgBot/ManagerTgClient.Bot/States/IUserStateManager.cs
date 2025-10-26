namespace Manager.ManagerTgClient.Bot.States;

public interface IStateManager
{
    IState GetState(long userId);
    void SetState(IState state, long userId);
}