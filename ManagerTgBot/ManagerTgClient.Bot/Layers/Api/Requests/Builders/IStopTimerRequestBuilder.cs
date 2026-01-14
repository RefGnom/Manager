namespace Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;

public interface IStopTimerRequestBuilder: IResourceRequestBuilder<StopTimerRequest, IStopTimerRequestBuilder>
{
    IStopTimerRequestBuilder WithNowStopTime();
}