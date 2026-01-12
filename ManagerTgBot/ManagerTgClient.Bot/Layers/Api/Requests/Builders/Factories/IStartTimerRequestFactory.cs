namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders.Factories;

public interface IStartTimerRequestFactory
{
    StartTimerRequest Create(long userId, string name, DateTime startTime, TimeSpan? pingTimeout);
}