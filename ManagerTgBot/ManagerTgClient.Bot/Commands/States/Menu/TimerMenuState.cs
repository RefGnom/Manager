using Manager.ManagerTgClient.Bot.Commands.States.Commands.StartTimer;
using Manager.ManagerTgClient.Bot.Commands.States.Commands.StopTimer;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Commands.States.Menu;

public class TimerMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : MenuStateBase(botClient, stateManager)
{
    private const string StartTimer = "/startTimer";
    private const string StopTimer = "/stopTimer";

    protected override Dictionary<string, Type> States => new()
    {
        { StartTimer, typeof(EnteringTimerNameForStartState) },
        { StopTimer, typeof(EnteringTimerNameForStopState) },
    };

    protected override string MessageToSend => "Выберите действие";

    protected override InlineKeyboardMarkup InlineKeyboard =>
        new InlineKeyboardMarkup().AddButton("Запустить таймер", callbackData: StartTimer)
            .AddButton("Остановить таймер", callbackData: StopTimer);
}