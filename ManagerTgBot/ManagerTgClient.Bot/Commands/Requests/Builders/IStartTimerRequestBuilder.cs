namespace Manager.ManagerTgClient.Bot.Commands.Requests.Builders;

public interface IStartTimerRequestBuilder
{
    IStartTimerRequestBuilder Initialize();
    StartTimerRequest Build();
}