using Manager.Core.Networking;

namespace Manager.TimerService.Client;

public interface ITimerServiceApiClientFactory
{
    ITimerServiceApiClient Create(string apiKey);
}

public class TimerServiceApiClientFactory(
    IPortProvider portProvider,
    IResilientHttpClientFactory resilientHttpClientFactory
) : ITimerServiceApiClientFactory
{
    public ITimerServiceApiClient Create(string apiKey)
    {
        var port = portProvider.GetPort("TIMER_SERVICE_PORT");
        var url = $"http://localhost:{port}";
        var httpClient = resilientHttpClientFactory.CreateClient(url, apiKey);
        return new TimerServiceApiClient(httpClient);
    }
}