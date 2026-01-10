using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StartTimer;

public class EnteringTimerDescriptionState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : StateBase(botClient, stateManager)
{
    protected override InlineKeyboardMarkup InlineKeyboard => throw new NotImplementedException();
    protected override string MessageToSend => throw new NotImplementedException();
    protected override UpdateType[] SupportedUpdateType => throw new NotImplementedException();
    public override Task ProcessUpdateAsync(Update update) => throw new NotImplementedException();
}