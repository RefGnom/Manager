namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders.Factories;

public class StartTimerRequestFactory() : IStartTimerRequestFactory
{
    public StartTimerRequest Create(long userId, string name, DateTime startTime, TimeSpan? pingTimeOut = null)
    {
        return new StartTimerRequest(userId, name,  startTime, pingTimeOut);
    }
}