using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public abstract class StateBase(
    ITelegramBotClient botClient
) : IState
{
    protected abstract InlineKeyboardMarkup InlineKeyboard { get; }
    protected abstract string MessageToSend { get; }

    protected abstract UpdateType[] SupportedUpdateType { get; }
    public abstract Task ProcessUpdateAsync(Update update);

    public async Task InitializeAsync(long userId) => await botClient.SendMessage(
        userId,
        MessageToSend,
        replyMarkup: InlineKeyboard
    );
}