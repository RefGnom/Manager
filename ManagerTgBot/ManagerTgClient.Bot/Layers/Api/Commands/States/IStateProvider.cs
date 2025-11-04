namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public interface IStateProvider
{
    IState GetState(Type stateType);
}