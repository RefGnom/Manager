using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Menu;
using Manager.ManagerTgClient.Bot.Layers.Api.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StartTimer;

public class EnteringTimerDescriptionState(
    ITelegramBotClient botClient,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : StateBase(botClient, stateManager)
{
    protected override string MessageToSend => "Здарова";

    public override async Task ProcessUpdateAsync(Update update)
    {
        if (!IsSupportedUpdate(update))
        {
            throw new NotSupportedUpdateTypeException(update.Type.ToString());
        }

        builder.WithDescription(update.Message!.Text!);
        await StateManager.SetStateAsync(update.Message!.From!.Id, typeof(MainMenuState));
        var request = builder.Build();
        await BotClient.SendMessage(
            update.Message.From.Id,
            $"{request.UserId.ToString()} {request.Name} {request.Description}"
        );
    }
}