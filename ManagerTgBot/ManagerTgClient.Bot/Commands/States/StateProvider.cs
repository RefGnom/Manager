using Manager.ManagerTgClient.Bot.Commands.States.Menu;

namespace Manager.ManagerTgClient.Bot.Commands.States;

public class StateProvider(
    IEnumerable<IState> states
) : IStateProvider
{
    public IState GetState(Type stateType)
    {
        return stateType == typeof(IState)
            ? states.First(s => s.GetType() == typeof(MainMenuState))
            : states.First(s => s.GetType() == stateType);
    }
}