using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StopTimer;

public class EnteringTimerNameForStopState : IState
{
    public Task ProcessUpdateAsync(Update update) => throw new NotImplementedException();

    public Task InitializeAsync(long userId) => throw new NotImplementedException();
    public Task SetNextStateAsync(long userId, IState nextState) => throw new NotImplementedException();
}