using Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.Start;

public class EnteringTimerNameForStartState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : StateBase(botInteractionService, stateManager)
{
    protected override string MessageToSend => "Введите название своего таймера";

    protected override async Task HandleUpdateAsync(Update update)
    {
        builder.ForUser(update.GetUserId()).WithName(update.Message!.Text!);
        await MoveToNextStateAsync(
            update,
            new AskStartTimeIsNowState(BotInteractionService, StateManager, builder)
        );
    }
}