using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public class TimerMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : MenuStateBase(botClient, stateManager)
{
    private const string StartTimer = "/startTimer";
    private const string StopTimer = "/stopTimer";

    protected override InlineKeyboardMarkup InlineKeyboard =>
        new InlineKeyboardMarkup().AddButton("Запустить таймер", callbackData: StartTimer)
            .AddButton("Остановить таймер", callbackData: StopTimer);

    protected override string MessageToSend => "Выберите действие";

    protected override Dictionary<string, Type> States { get; }

    protected override UpdateType[] SupportedUpdateType { get; }
}