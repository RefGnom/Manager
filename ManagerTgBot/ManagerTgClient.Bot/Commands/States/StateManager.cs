using Manager.ManagerTgClient.Bot.Commands.States.Menu;

namespace Manager.ManagerTgClient.Bot.Commands.States;

public class StateManager(
    Lazy<IStateProvider> stateProvider
) : IStateManager
{
    private readonly Dictionary<long, IState> _states = new();

    public async Task<IState> GetStateAsync(long chatId)
    {
        _states.TryGetValue(chatId, out var state);
        if (state is null)
        {
            state = stateProvider.Value.GetState(typeof(MainMenuState));
            await SetStateAsync(chatId, typeof(MainMenuState));
        }

        return state;
    }

    public async Task SetStateAsync(long chatId, Type stateType)
    {
        var state = stateProvider.Value.GetState(stateType);
        _states[chatId] = state;
        await state.InitializeAsync(chatId);
    }
}