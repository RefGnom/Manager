using Manager.ManagerTgClient.Bot.Extentions;
using Manager.ManagerTgClient.Bot.States.Commands.StartTimer;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public class TimerMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager,
    IStateProvider stateprovider
) : StateBase(botClient)
{
    protected override InlineKeyboardMarkup InlineKeyboard { get; }
    protected override string MessageToSend { get; }

    public override Task ProcessUpdateAsync(Update update)
    {
        throw new NotImplementedException();
    }
}