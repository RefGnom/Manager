namespace Manager.ManagerTgClient.Bot.States;

public interface IStateProvider
{
    IState GetState<TState>() where TState : IState;
}