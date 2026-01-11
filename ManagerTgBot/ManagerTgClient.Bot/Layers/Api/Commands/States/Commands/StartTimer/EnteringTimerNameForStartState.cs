using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.Exceptions;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StartTimer;

public class EnteringTimerNameForStartState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : StateBase(botInteractionService, stateManager)
{
    protected override string MessageToSend => "Введите название своего таймера";

    public override async Task ProcessUpdateAsync(Update update)
    {
        if (!IsSupportedUpdate(update))
        {
            throw new NotSupportedUpdateTypeException(update.Type.ToString());
        }

        var userId = update.Message!.From!.Id;
        builder.ForUser(userId).WithName(update.Message!.Text!);
        await SetNextStateAsync(
            userId,
            new EnteringTimerDescriptionState(BotInteractionService, StateManager, builder)
        );
    }
}