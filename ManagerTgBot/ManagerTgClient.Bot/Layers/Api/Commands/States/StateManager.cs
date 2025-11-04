using Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Menu;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public class StateManager(
    Lazy<IStateProvider> stateProvider
) : IStateManager
{
    private static readonly Type mainMenuType = typeof(MainMenuState);
    private readonly Dictionary<long, IState> states = new();

    public async Task<IState> GetStateAsync(long userId)
    {
        states.TryGetValue(userId, out var state);
        if (state is not null)
        {
            return state!;
        }

        await SetStateAsync(userId, mainMenuType);
        return states[userId];
    }

    public async Task SetStateAsync(long userId, Type stateType)
    {
        var state = stateProvider.Value.GetState(stateType);
        states[userId] = state;
        await state.InitializeAsync(userId);
    }
}