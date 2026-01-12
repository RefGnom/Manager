using Manager.Core.Common.Time;
using Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders.Factories;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public class StartTimerRequestBuilder(IStartTimerRequestFactory factory, IDateTimeProvider dateTimeProvider) : IStartTimerRequestBuilder
{
    private long userId;
    private string? name;
    private DateTime startTime;
    private TimeSpan? pingTimeOut;
    public StartTimerRequest Build() => factory.Create(userId, name!, startTime, pingTimeOut);

    public IStartTimerRequestBuilder ForUser(long data)
    {
        userId = data;
        return this;
    }

    public void WithName(string data) => name = data;

    public void WithCustomStartTime(DateTime data) => startTime = data;
    public void WithCurrentStartTime() => startTime = dateTimeProvider.UtcNow;
    public void WithDefaultPingTimeout() => pingTimeOut = TimeSpan.FromMinutes(5);
    public void WithPingTimeout(TimeSpan data) => pingTimeOut = data;
}