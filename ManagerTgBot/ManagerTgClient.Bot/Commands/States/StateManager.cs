using Manager.ManagerTgClient.Bot.Commands.States.Menu;

namespace Manager.ManagerTgClient.Bot.Commands.States;

public class StateManager(
    Lazy<IStateProvider> stateProvider
) : IStateManager
{
    private readonly Dictionary<long, IState> states = new();

    public async Task<IState> GetStateAsync(long userId)
    {
        states.TryGetValue(userId, out var state);
        if (state is null)
        {
            await SetStateAsync(userId, typeof(MainMenuState));
        }

        return state;
    }

    public async Task SetStateAsync(long userId, Type stateType)
    {
        var state = stateProvider.Value.GetState(stateType);
        states[userId] = state;
        await state.InitializeAsync(userId);
    }
}