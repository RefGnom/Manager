using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StartTimer;

public class EnteringTimerDescriptionState(
    ITelegramBotClient botClient,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : StateBase(botClient, stateManager)
{
    protected override InlineKeyboardMarkup InlineKeyboard { get; }
    protected override string MessageToSend { get; }
    protected override UpdateType[] SupportedUpdateType { get; }
    public override Task ProcessUpdateAsync(Update update) => throw new NotImplementedException();
}