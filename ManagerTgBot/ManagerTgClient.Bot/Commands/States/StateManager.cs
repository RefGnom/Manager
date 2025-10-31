using Manager.ManagerTgClient.Bot.Commands.States.Menu;

namespace Manager.ManagerTgClient.Bot.Commands.States;

public class StateManager(
    Lazy<IStateProvider> stateProvider
) : IStateManager
{
    private readonly Dictionary<long, IState> _states = new();

    public IState GetState(long chatId)
    {
        _states.TryGetValue(chatId, out var state);
        if (state is null)
        {
            state = stateProvider.Value.GetState(typeof(MainMenuState));
            SetState<MainMenuState>(chatId);
        }

        state.InitializeAsync(chatId);
        return state;
    }

    public void SetState<TState>(long chatId) where TState : IState
    {
        var state = stateProvider.Value.GetState(typeof(TState));
        _states[chatId] = state;
    }

    public void SetState(long chatId, Type stateType)
    {
        var state = stateProvider.Value.GetState(stateType);
        _states[chatId] = state;
    }
}