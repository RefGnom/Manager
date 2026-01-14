using Manager.Core.Common.Time;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public class StopTimerRequestBuilder(
    IDateTimeProvider dateTimeProvider
) : IStopTimerRequestBuilder
{
    private long userId;
    private string? name;
    private DateTime? stopTime;

    public StopTimerRequest Build() => IsValidRequest()
        ? new StopTimerRequest(userId, name!, stopTime.GetValueOrDefault(dateTimeProvider.UtcNow))
        : throw new Exception("Invalid request");

    public IStopTimerRequestBuilder ForUser(long value)
    {
        userId = value;
        return this;
    }

    public IStopTimerRequestBuilder WithName(string value)
    {
        name = value;
        return this;
    }

    public IStopTimerRequestBuilder WithNowStopTime()
    {
        stopTime = DateTime.Now;
        return this;
    }

    private bool IsValidRequest() => userId is not 0 && name is not null;
}