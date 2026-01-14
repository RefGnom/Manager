using Manager.Core.Common.Time;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public class StartTimerRequestBuilder(
    IDateTimeProvider dateTimeProvider
) : IStartTimerRequestBuilder
{
    private long userId;
    private string? name;
    private DateTime? startTime;
    private TimeSpan? pingTimeOut;

    public StartTimerRequest Build() => IsValidRequest()
        ? new StartTimerRequest(userId, name!, startTime!.Value, pingTimeOut)
        : throw new InvalidOperationException("Invalid request");

    public IStartTimerRequestBuilder ForUser(long value)
    {
        userId = value;
        return this;
    }

    public IStartTimerRequestBuilder WithName(string value)
    {
        name = value;
        return this;
    }

    public IStartTimerRequestBuilder WithCustomStartTime(DateTime value)
    {
        startTime = value;
        return this;
    }

    public IStartTimerRequestBuilder WithCurrentStartTime()
    {
        startTime = dateTimeProvider.UtcNow;
        return this;
    }

    public IStartTimerRequestBuilder WithDefaultPingTimeout()
    {
        pingTimeOut = TimeSpan.FromMinutes(5);
        return this;
    }

    public IStartTimerRequestBuilder WithPingTimeout(TimeSpan value)
    {
        pingTimeOut = value;
        return this;
    }

    private bool IsValidRequest() => userId != 0 && name is not null && startTime is not null;
}