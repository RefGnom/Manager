using Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.Start;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.Stop;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Menu;

public class TimerMenuState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager
) : MenuStateBase(botInteractionService, stateManager)
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