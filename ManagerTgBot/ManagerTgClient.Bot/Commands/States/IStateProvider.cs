namespace Manager.ManagerTgClient.Bot.Commands.States;

public interface IStateProvider
{
    IState GetState(Type stateType);

}