using Manager.ManagerTgClient.Bot.Extentions;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public class MainMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : MenuStateBase(botClient, stateManager)
{
    private const string TimerMenu = "/timers";
    private const string AccountMenu = "/accounts";
    private static readonly UpdateType[] supportedUpdateType = [UpdateType.Message, UpdateType.CallbackQuery];

    protected override string MessageToSend => "Выберите функцию";

    protected override InlineKeyboardMarkup InlineKeyboard => new InlineKeyboardMarkup()
        .AddButton(InlineKeyboardButton.WithCallbackData("Таймеры", TimerMenu))
        .AddButton(InlineKeyboardButton.WithCallbackData("Настройка аккаунта", AccountMenu));

    protected override Dictionary<string, Type> States { get; }

    public override Task ProcessUpdateAsync(Update update)
    {
        if (!supportedUpdateType.Contains(update.Type))
        {
            throw new Exception();
        }

        var value = update.GetUserData();
        switch (value)
        {
            case TimerMenu:
                stateManager.SetState<TimerMenuState>(update.Message!.Chat.Id);
                break;
            case AccountMenu:
                stateManager.SetState<AccountMenuState>(update.Message!.Chat.Id);
                break;
            default:
                Console.WriteLine(value);
                break;
        }

        return Task.CompletedTask;
    }

    protected override UpdateType[] SupportedUpdateType { get; }
}