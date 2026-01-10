namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders.Factories;

public interface IStartTimerRequestFactory
{
    StartTimerRequest Create(long userId, string name, string description);
}