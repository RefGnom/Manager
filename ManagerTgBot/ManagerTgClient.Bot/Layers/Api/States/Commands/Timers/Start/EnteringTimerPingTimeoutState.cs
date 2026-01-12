using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.Start;

public class EnteringTimerPingTimeoutState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager
) : StateBase(botInteractionService, stateManager)
{
    protected override string MessageToSend => "Введите ping timeout";
    protected override Task HandleUpdateAsync(Update update) => throw new NotImplementedException();
}