namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public interface IGetUserTimersBuilder : IRequestBuilder<GetUserTimersRequest>
{
    IGetUserTimersBuilder ForUser(long value);
    IGetUserTimersBuilder WithArchived();
    IGetUserTimersBuilder WithDeleted();
}