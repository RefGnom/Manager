namespace Manager.ManagerTgClient.Bot.Commands.Requests;

public class StartTimerRequest(long chatId, string timerName) : ICommandRequest
{
    public string TimerName = timerName;
    public long ChatId = chatId;
}