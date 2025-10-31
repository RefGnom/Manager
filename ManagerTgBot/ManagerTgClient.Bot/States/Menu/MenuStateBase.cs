using Manager.ManagerTgClient.Bot.Extentions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public abstract class MenuStateBase(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : StateBase(botClient)
{
    protected abstract Dictionary<string, Type> States { get; }

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

        stateManager.SetState(chatId, state);
        return Task.CompletedTask;
    }
}