using Manager.ManagerTgClient.Bot.Extentions;
using Manager.ManagerTgClient.Bot.States.Commands.StartTimer;
using Manager.ManagerTgClient.Bot.States.Commands.StopTimer;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.States.Menu;

public class TimerMenuState(
    ITelegramBotClient botClient,
    IStateManager stateManager
) : StateBase(botClient)
{
    private const string StartTimer = "/startTimer";
    private const string StopTimer = "/stopTimer";

    protected override InlineKeyboardMarkup InlineKeyboard =>
        new InlineKeyboardMarkup().AddButton("Запустить таймер", callbackData: StartTimer)
            .AddButton("Остановить таймер", callbackData: StopTimer);

    protected override string MessageToSend => "Выберите действие";

    public override Task ProcessUpdateAsync(Update update)
    {
        var userData = update.GetUserData();
        var chatId = update.GetChatId();
        switch (userData)
        {
            case StartTimer:
            {
                stateManager.SetState<EnteringTimerNameForStartState>(chatId);
                break;
            }
            case StopTimer:
            {
                stateManager.SetState<EnteringTimerNameForStopState>(chatId);
                break;
            }
            default:
            {
                Console.WriteLine();
                break;
            }
        }

        return Task.CompletedTask;
    }
}