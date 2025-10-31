using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public class MainMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : MenuStateBase(botClient, stateManager)
{
    private const string TimerMenu = "/timers";
    private const string AccountMenu = "/accounts";

    protected override Dictionary<string, Type> States => new()
    {
        { TimerMenu, typeof(TimerMenuState) },
        { AccountMenu, typeof(AccountMenuState) },
    };

    protected override string MessageToSend => "Выберите функцию";

    protected override InlineKeyboardMarkup InlineKeyboard => new InlineKeyboardMarkup()
        .AddButton(InlineKeyboardButton.WithCallbackData("Таймеры", TimerMenu))
        .AddButton(InlineKeyboardButton.WithCallbackData("Настройка аккаунта", AccountMenu));
}