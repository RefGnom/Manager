using Manager.ManagerTgClient.Bot.Layers.Api.Exceptions;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Menu;

public abstract class MenuStateBase(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : StateBase(botClient, stateManager)
{
    protected abstract Dictionary<string, Type> States { get; }
    protected override UpdateType[] SupportedUpdateType => [UpdateType.Message, UpdateType.CallbackQuery];

    public override async Task ProcessUpdateAsync(Update update)
    {
        if (!SupportedUpdateType.Contains(update.Type))
        {
            throw new NotSupportedUpdateTypeException($"{update.Type} не поддерживается.");
        }

        if (update.Type is UpdateType.CallbackQuery)
        {
            await BotClient.AnswerCallbackQuery(update.CallbackQuery!.Id);
        }

        var userData = update.GetUserData()!;
        var userId = update.GetUserId();
        if (!States.TryGetValue(userData, out var state))
        {
            return;
        }

        await StateManager.SetStateAsync(userId, state);
    }
}