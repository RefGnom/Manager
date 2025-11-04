using Manager.ManagerTgClient.Bot.Commands.States.Menu;

namespace Manager.ManagerTgClient.Bot.Commands.States;

public class StateProvider(
    IEnumerable<IState> states
) : IStateProvider
{
    private static readonly Type mainMenuType = typeof(MainMenuState);
    private static readonly Type stateInterface = typeof(IState);
    public IState GetState(Type stateType)
    {
        return stateType == stateInterface
            ? states.First(s => s.GetType() == mainMenuType)
            : states.First(s => s.GetType() == stateType);
    }
}