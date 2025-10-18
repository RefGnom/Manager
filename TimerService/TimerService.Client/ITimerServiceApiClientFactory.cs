using Manager.Core.Networking;

namespace Manager.TimerService.Client;

public interface ITimerServiceApiClientFactory
{
    ITimerServiceApiClient Create(string apiKey);
}

public class TimerServiceApiClientFactory(
    IPortProvider portProvider
) : ITimerServiceApiClientFactory
{
    public ITimerServiceApiClient Create(string apiKey)
    {
        var port = portProvider.GetPort("TIMER_SERVICE_PORT");
        var url = $"http://localhost:{port}";
        return new TimerServiceApiClient(apiKey, url);
    }
}