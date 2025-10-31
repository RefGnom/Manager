namespace Manager.ManagerTgClient.Bot.States;

public interface IStateProvider
{
    IState GetState(Type stateType);

}