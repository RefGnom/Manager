using Manager.ManagerTgClient.Bot.Layers.Services;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States;

public abstract class StateBase(
    IBotInteractionService botInteractionService,
    IStateManager stateManager
) : IState
{
    protected IStateManager StateManager => stateManager;
    protected IBotInteractionService BotInteractionService => botInteractionService;
    protected virtual InlineKeyboardMarkup? InlineKeyboard => null;
    protected abstract string MessageToSend { get; }

    private static UpdateType[] SupportedUpdateType => [UpdateType.Message, UpdateType.CallbackQuery];
    public abstract Task ProcessUpdateAsync(Update update);

    public async Task InitializeAsync(long userId) => await botInteractionService.SendMessageAsync(
        userId,
        MessageToSend,
        replyMarkup: InlineKeyboard
    );

    protected async Task SetNextStateAsync(long userId, IState nextState) =>
        await stateManager.SetStateAsync(userId, nextState);

    protected static bool IsSupportedUpdate(Update update) => SupportedUpdateType.Contains(update.Type);
}