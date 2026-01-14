namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public class GetUserTimersBuilder : IGetUserTimersBuilder
{
    private long userId;
    private bool archived;
    private bool deleted;

    public GetUserTimersRequest Build() => IsValidRequest()
        ? new GetUserTimersRequest(userId, archived, deleted)
        : throw new InvalidOperationException("Invalid request");

    public IGetUserTimersBuilder ForUser(long value)
    {
        userId = value;
        return this;
    }

    public IGetUserTimersBuilder WithArchived()
    {
        archived = true;
        return this;
    }

    public IGetUserTimersBuilder WithDeleted()
    {
        deleted = true;
        return this;
    }

    private bool IsValidRequest() => userId > 0;
}