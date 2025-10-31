using Manager.ManagerTgClient.Bot.States.Menu;

namespace Manager.ManagerTgClient.Bot.States;

public class StateManager(
    Lazy<IStateProvider> stateProvider
) : IStateManager
{
    private readonly Dictionary<long, IState> _states = new();

    public IState GetState(long userId)
    {
        _states.TryGetValue(userId, out var state);
        if (state is null)
        {
            state = stateProvider.Value.GetState<MainMenuState>();
            SetState<MainMenuState>(userId);
        }

        return state;
    }

    public void SetState<TState>(long userId) where TState : IState
    {
        var state = stateProvider.Value.GetState<TState>();
        _states[userId] = state;
    }

    public void SetState(long userId, Type stateType)
    {
        throw new NotImplementedException();
    }
}