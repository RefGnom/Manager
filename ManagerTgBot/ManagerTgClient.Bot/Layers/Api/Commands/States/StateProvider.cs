using Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Menu;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public class StateProvider(
    IEnumerable<IState> states
) : IStateProvider
{
    private static readonly Type stateInterface = typeof(IState);

    public Type DefaultStateType => typeof(MainMenuState);
    public bool TryGetState(Type stateType, out IState state) => throw new NotImplementedException();

    public IState GetState(Type stateType)
    {
        return stateType == stateInterface
            ? states.First(s => s.GetType() == DefaultStateType)
            : states.First(s => s.GetType() == stateType);
    }
}