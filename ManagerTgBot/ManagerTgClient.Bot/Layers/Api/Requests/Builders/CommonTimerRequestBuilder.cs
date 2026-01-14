namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public class CommonTimerRequestBuilder : ICommonTimerRequestBuilder
{
    private long userId;
    private string? name;

    public CommonTimerRequest Build() => IsValidRequest()
        ? new CommonTimerRequest(userId, name!)
        : throw new InvalidOperationException();

    public ICommonTimerRequestBuilder ForUser(long value)
    {
        userId = value;
        return this;
    }

    public ICommonTimerRequestBuilder WithName(string value)
    {
        name = value;
        return this;
    }

    private bool IsValidRequest() => userId > 0 && name != null;
}