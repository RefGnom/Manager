using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Commands.States;

public abstract class StateBase(ITelegramBotClient botClient): IState
{
    protected abstract InlineKeyboardMarkup InlineKeyboard { get; }
    protected abstract string MessageToSend { get; }
    public abstract Task ProcessUpdateAsync(Update update);

    protected abstract UpdateType[] SupportedUpdateType { get; }
    public Task InitializeAsync(long chatId) => botClient.SendMessage(
        chatId,
        MessageToSend,
        replyMarkup: InlineKeyboard
    );
}