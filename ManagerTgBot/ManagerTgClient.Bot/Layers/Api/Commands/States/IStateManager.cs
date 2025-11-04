namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public interface IStateManager
{
    Task<IState> GetStateAsync(long userId);
    Task SetStateAsync(long userId, Type stateType);
}