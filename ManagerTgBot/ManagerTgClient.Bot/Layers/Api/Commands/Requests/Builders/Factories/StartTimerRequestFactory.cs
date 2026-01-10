namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders.Factories;

public class StartTimerRequestFactory: IStartTimerRequestFactory
{
    public StartTimerRequest Create(long userId, string name, string description)
    {
        return new StartTimerRequest(userId, name, description);
    }
}