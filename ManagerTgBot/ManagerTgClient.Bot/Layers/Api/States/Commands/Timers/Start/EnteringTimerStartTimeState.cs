using Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.Start;

public class EnteringTimerStartTimeState(
    IBotInteractionService botClient,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : StateBase(botClient, stateManager)
{
    protected override string MessageToSend =>
        "Вы хотите запустить таймер прямо сейчас? Если нет введите через сколько времени таймер должен быть запущен";

    protected override async Task HandleUpdateAsync(Update update)
    {
        builder.WithCurrentStartTime();
        var request = builder.Build();
        await BotInteractionService.SendMessageAsync(
            update.GetUserId(),
            $"{request.UserId.ToString()} {request.Name} {request.StartTime}"
        );
        await StateManager.SetStateAsync(update.GetUserId(), typeof(AskPingTimeoutState));
    }
}