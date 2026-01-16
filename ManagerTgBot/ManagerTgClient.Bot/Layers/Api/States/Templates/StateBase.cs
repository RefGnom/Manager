using Manager.ManagerTgClient.Bot.Layers.Api.Exceptions;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;

public abstract class StateBase(
    IBotInteractionService botInteractionService,
    IStateManager stateManager
) : IState
{
    protected IBotInteractionService BotInteractionService => botInteractionService;
    protected IStateManager StateManager => stateManager;
    protected virtual InlineKeyboardMarkup? InlineKeyboard => null;
    protected abstract string MessageToSend { get; }

    protected virtual UpdateType[] SupportedUpdateType => [UpdateType.Message, UpdateType.CallbackQuery];

    public async Task ProcessUpdateAsync(Update update)
    {
        if (!IsSupportedUpdate(update))
        {
            throw new NotSupportedUpdateTypeException($"{update.Type} is not supported");
        }

        await HandleUpdateAsync(update);
    }
    protected abstract Task HandleUpdateAsync(Update update);
    public async Task InitializeAsync(long userId) => await botInteractionService.SendMessageAsync(
        userId,
        MessageToSend,
        replyMarkup: InlineKeyboard
    );

    protected async Task MoveToNextStateAsync(Update update, IState nextState)
    {
        if (update.Type is UpdateType.CallbackQuery)
        {
            await BotInteractionService.AnswerCallbackQueryAsync(update.CallbackQuery!.Id);
        }

        var userId = update.GetUserId();
        await StateManager.SetStateAsync(userId, nextState);
    }

    protected bool IsSupportedUpdate(Update update) => SupportedUpdateType.Contains(update.Type);
}