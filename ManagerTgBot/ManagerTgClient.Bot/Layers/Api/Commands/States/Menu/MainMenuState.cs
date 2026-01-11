using Manager.ManagerTgClient.Bot.Layers.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Menu;

public class MainMenuState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager
) : MenuStateBase(botInteractionService, stateManager)
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
        .AddButton("Таймеры", TimerMenu)
        .AddButton("Настройка аккаунта", AccountMenu);
}