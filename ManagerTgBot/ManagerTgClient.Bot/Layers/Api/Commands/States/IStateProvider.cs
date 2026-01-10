namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public interface IStateProvider
{
    Type DefaultStateType { get; }
    IState GetState(Type stateType);
}