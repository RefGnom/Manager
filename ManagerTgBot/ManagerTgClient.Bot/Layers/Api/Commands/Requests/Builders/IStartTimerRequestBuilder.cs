namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;

public interface IStartTimerRequestBuilder
{
    IStartTimerRequestBuilder ForUser(long userId);
    void WithName(string name);
    void WithDescription(string description);
    StartTimerRequest Build();
}