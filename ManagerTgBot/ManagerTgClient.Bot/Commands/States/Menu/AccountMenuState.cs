using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Commands.States.Menu;

public class AccountMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : MenuStateBase(botClient, stateManager)
{
    private const string CreateAccount = "/createAccount";
    private const string GetAccountInfo = "/getAccount";

    protected override InlineKeyboardMarkup InlineKeyboard => new InlineKeyboardMarkup()
        .AddButton(InlineKeyboardButton.WithCallbackData("Создать аккаунт", CreateAccount))
        .AddButton(InlineKeyboardButton.WithCallbackData("Получить информацию об аккаунте", GetAccountInfo));

    protected override string MessageToSend => "Выберите действие";

    protected override Dictionary<string, Type> States => new()
    {
        { CreateAccount, typeof(StateBase) },
        { GetAccountInfo, typeof(StateBase) },
    };
}