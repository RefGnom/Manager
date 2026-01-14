namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public interface IResourceRequestBuilder<out TRequest, out TRequestBuilder> : IRequestBuilder<TRequest>
    where TRequestBuilder : IResourceRequestBuilder<TRequest, TRequestBuilder>
{
    TRequestBuilder ForUser(long value);
    TRequestBuilder WithName(string value);
}