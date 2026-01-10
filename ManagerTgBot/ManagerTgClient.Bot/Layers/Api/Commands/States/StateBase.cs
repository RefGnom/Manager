using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public abstract class StateBase(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : IState
{
    protected IStateManager StateManager => stateManager;
    protected ITelegramBotClient BotClient => botClient;
    protected virtual InlineKeyboardMarkup? InlineKeyboard => null;
    protected abstract string MessageToSend { get; }

    private static UpdateType[] SupportedUpdateType => [UpdateType.Message, UpdateType.CallbackQuery];
    public abstract Task ProcessUpdateAsync(Update update);

    public async Task InitializeAsync(long userId) => await botClient.SendMessage(
        userId,
        MessageToSend,
        replyMarkup: InlineKeyboard
    );

    protected async Task SetNextStateAsync(long userId, IState nextState) =>
        await stateManager.SetStateAsync(userId, nextState);

    protected bool IsSupportedUpdate(Update update) => SupportedUpdateType.Contains(update.Type);
}