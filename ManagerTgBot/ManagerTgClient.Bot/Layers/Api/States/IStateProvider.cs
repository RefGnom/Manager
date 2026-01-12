namespace Manager.ManagerTgClient.Bot.Layers.Api.States;

public interface IStateProvider
{
    Type DefaultStateType { get; }
    IState GetState(Type stateType);
}