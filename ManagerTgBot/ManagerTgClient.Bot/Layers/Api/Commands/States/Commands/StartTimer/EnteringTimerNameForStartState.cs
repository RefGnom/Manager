using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StartTimer;

public class EnteringTimerNameForStartState(
    ITelegramBotClient botClient,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : StateBase(botClient, stateManager)
{
    private readonly IStateManager stateManager = stateManager;
    private readonly ITelegramBotClient botClient = botClient;
    protected override InlineKeyboardMarkup InlineKeyboard => throw new NotImplementedException();
    protected override string MessageToSend => "Введите название своего таймера";
    protected override UpdateType[] SupportedUpdateType => [UpdateType.Message];

    public override async Task ProcessUpdateAsync(Update update)
    {
        if (!IsSupportedUpdate(update))
        {
            throw new NotSupportedUpdateTypeException(update.Type.ToString());
        }

        builder.WithName(update.Message!.Text!);
        await SetNextStateAsync(
            update.Message!.From!.Id,
            new EnteringTimerDescriptionState(botClient, stateManager)
        );
    }
}