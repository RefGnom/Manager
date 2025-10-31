namespace Manager.ManagerTgClient.Bot.Commands.States;

public interface IStateManager
{
    IState GetState(long chatId);
    void SetState<TState>(long chatId) where TState : IState;
    void SetState(long chatId, Type stateType);
}