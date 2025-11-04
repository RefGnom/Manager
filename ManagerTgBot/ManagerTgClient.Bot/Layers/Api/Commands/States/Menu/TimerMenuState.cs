using Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StartTimer;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StopTimer;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Menu;

public class TimerMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : MenuStateBase(botClient, stateManager)
{
    private const string StartTimer = "/startTimer";
    private const string StopTimer = "/stopTimer";
    private const string Exit = "/exit";


    protected override Dictionary<string, Type> States => new()
    {
        { StartTimer, typeof(EnteringTimerNameForStartState) },
        { StopTimer, typeof(EnteringTimerNameForStopState) },
        { Exit, typeof(MainMenuState) },
    };

    protected override string MessageToSend => "Выберите действие";

    protected override InlineKeyboardMarkup InlineKeyboard =>
        new InlineKeyboardMarkup()
            .AddButton("Запустить таймер", StartTimer)
            .AddButton("Остановить таймер", StopTimer)
            .AddButton("В главное меню", Exit);
}