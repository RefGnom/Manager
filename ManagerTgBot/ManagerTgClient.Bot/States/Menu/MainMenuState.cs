using Manager.ManagerTgClient.Bot.Extentions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public class MainMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager,
    IStateProvider stateProvider
) : StateBase(botClient)
{
    private const string TimerMenu = "/timers";
    private const string AccountMenu = "/accounts";
    private static readonly UpdateType[] supportedUpdateType = [UpdateType.Message, UpdateType.CallbackQuery];

    protected override string MessageToSend => "Выберите функцию";

    protected override InlineKeyboardMarkup InlineKeyboard => new InlineKeyboardMarkup()
        .AddButton(InlineKeyboardButton.WithCallbackData("Таймеры", TimerMenu))
        .AddButton(InlineKeyboardButton.WithCallbackData("Настройка аккаунта", AccountMenu));

    public override Task ProcessUpdateAsync(Update update)
    {
        if (!supportedUpdateType.Contains(update.Type))
        {
            throw new Exception();
        }

        var value = update.GetUserData();
        switch (value)
        {
            case "/timers":
                stateManager.SetState(update.Message!.Chat.Id, stateProvider.GetState<TimerMenuState>());
                break;
            case "/accounts":
                stateManager.SetState(update.Message!.Chat.Id, stateProvider.GetState<AccountMenuState>());
                break;
            default:
                Console.WriteLine(value);
                break;
        }

        return Task.CompletedTask;
    }
}