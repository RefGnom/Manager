namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public interface IStartTimerRequestBuilder : IResourceRequestBuilder<StartTimerRequest, IStartTimerRequestBuilder>
{
    IStartTimerRequestBuilder WithCustomStartTime(DateTime startTime);
    IStartTimerRequestBuilder WithCurrentStartTime();
    IStartTimerRequestBuilder WithDefaultPingTimeout();
    IStartTimerRequestBuilder WithPingTimeout(TimeSpan pingTimeout);
}