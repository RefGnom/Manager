namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public class StateManager(
    Lazy<IStateProvider> stateProvider
) : IStateManager
{
    private readonly Dictionary<long, IState> states = new();

    public async Task<IState> GetStateAsync(long userId)
    {
        if (states.TryGetValue(userId, out var state))
        {
            return state!;
        }

        await SetStateAsync(userId, stateProvider.Value.DefaultStateType);
        return states[userId];
    }

    public async Task SetStateAsync(long userId, IState state)
    {
        states[userId] = state;
        await state.InitializeAsync(userId);
    }

    public async Task SetStateAsync(long userId, Type stateType)
    {
        var state = stateProvider.Value.GetState(stateType);
        await SetStateAsync(userId, state);
    }
}