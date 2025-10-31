using Manager.ManagerTgClient.Bot.Extentions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Manager.ManagerTgClient.Bot.Commands.States.Menu;

public abstract class MenuStateBase(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : StateBase(botClient)
{
    protected abstract Dictionary<string, Type> States { get; }
    protected override UpdateType[] SupportedUpdateType => [UpdateType.Message, UpdateType.CallbackQuery];

    public override Task ProcessUpdateAsync(Update update)
    {
        if (!SupportedUpdateType.Contains(update.Type))
        {
            throw new Exception();
        }

        var userData = update.GetUserData()!;
        var chatId = update.GetChatId();
        if (!States.TryGetValue(userData, out var state))
        {
            return Task.CompletedTask;
        }

        stateManager.SetStateAsync(chatId, state);
        return Task.CompletedTask;
    }
}