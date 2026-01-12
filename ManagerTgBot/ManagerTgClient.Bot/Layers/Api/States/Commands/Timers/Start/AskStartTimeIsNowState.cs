using Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.Start;

public class AskStartTimeIsNowState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : ChoiceStateBase(botInteractionService, stateManager)
{
    protected override string MessageToSend =>
        "Вы хотите запустить таймер прямо сейчас? Если нет введите через сколько времени таймер должен быть запущен";

    protected override async Task HandleUpdateAsync(Update update)
    {
        if (IsPositiveAnswer(update))
        {
            builder.WithCurrentStartTime();
            await SetNextStateAsync(
                update.GetUserId(),
                new AskPingTimeoutState(BotInteractionService, StateManager, builder)
            );
        }
        else
        {
            await SetNextStateAsync(
                update.GetUserId(),
                new EnteringTimerStartTimeState(BotInteractionService, StateManager, builder)
            );
        }
    }
}