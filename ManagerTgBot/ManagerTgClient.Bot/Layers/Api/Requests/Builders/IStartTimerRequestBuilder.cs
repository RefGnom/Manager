namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public interface IStartTimerRequestBuilder
{
    IStartTimerRequestBuilder ForUser(long userId);
    void WithName(string name);
    void WithCustomStartTime(DateTime startTime);
    void WithCurrentStartTime();
    void WithDefaultPingTimeout();
    void WithPingTimeout(TimeSpan pingTimeout);
    StartTimerRequest Build();
}