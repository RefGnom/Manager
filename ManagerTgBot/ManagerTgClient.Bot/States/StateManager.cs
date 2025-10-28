using Manager.ManagerTgClient.Bot.States.Menu;

namespace Manager.ManagerTgClient.Bot.States;

public class StateManager(IStateProvider stateProvider): IStateManager
{
    private readonly Dictionary<long, IState> _states = new();

    public IState GetState(long userId)
    {
        _states.TryGetValue(userId, out var state);
        if (state is null)
        {
            state = stateProvider.GetState<MainMenuState>();
            SetState(userId, state);
        }

        return state;
    }

    public void SetState(long userId, IState state)
    {
        _states[userId] = state;
    }
}