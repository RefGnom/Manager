namespace Manager.ManagerTgClient.Bot.States;

public class StateProvider(
    IEnumerable<IState> states
) : IStateProvider
{
    public IState GetState<TState>() where TState : IState
    {
        return states.First(s => s.GetType() == typeof(TState));
    }
}