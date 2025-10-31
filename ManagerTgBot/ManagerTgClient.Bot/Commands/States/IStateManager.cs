namespace Manager.ManagerTgClient.Bot.Commands.States;

public interface IStateManager
{
    Task<IState> GetStateAsync(long chatId);
    Task SetStateAsync(long chatId, Type stateType);
}