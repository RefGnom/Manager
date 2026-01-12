using Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Menu;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.Start;

public class AskPingTimeoutState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : ChoiceStateBase(botInteractionService, stateManager)
{
    protected override string MessageToSend => "Принять дефолтный пинг таймаут?";

    protected override async Task HandleUpdateAsync(Update update)
    {
        if (IsPositiveAnswer(update))
        {
            builder.WithDefaultPingTimeout();
            var request = builder.Build();
            await BotInteractionService.SendMessageAsync(
                update.GetUserId(),
                $"Таймер успешно создан {request.Name} {request.StartTime.Date} {request.PingTimeout!.Value.Minutes}"
            );
            await SetNextStateAsync(
                update.GetUserId(),
                new MainMenuState(BotInteractionService, StateManager)
            );
        }
        else
        {
            await SetNextStateAsync(
                update.GetUserId(),
                new EnteringTimerNameForStartState(BotInteractionService, StateManager, builder)
            );
        }
    }
}