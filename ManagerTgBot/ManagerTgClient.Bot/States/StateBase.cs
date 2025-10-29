using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.States;

public abstract class StateBase(ITelegramBotClient botClient): IState
{
    protected abstract InlineKeyboardMarkup InlineKeyboard { get; }
    protected abstract string MessageToSend { get; }
    public abstract Task ProcessUpdateAsync(Update update);

    public Task InitializeAsync(long chatId) => botClient.SendMessage(
        chatId,
        MessageToSend,
        replyMarkup: InlineKeyboard
    );
}