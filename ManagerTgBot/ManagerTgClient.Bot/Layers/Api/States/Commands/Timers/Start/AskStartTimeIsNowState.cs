using Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
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
            await MoveToNextStateAsync(
                update,
                new AskPingTimeoutState(BotInteractionService, StateManager, builder)
            );
        }
        else
        {
            await MoveToNextStateAsync(
                update,
                new EnteringTimerStartTimeState(BotInteractionService, StateManager, builder)
            );
        }
    }
}