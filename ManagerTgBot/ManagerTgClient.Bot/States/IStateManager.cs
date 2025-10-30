namespace Manager.ManagerTgClient.Bot.States;

public interface IStateManager
{
    IState GetState(long userId);
    void SetState<TState>(long userId) where TState : IState;
}