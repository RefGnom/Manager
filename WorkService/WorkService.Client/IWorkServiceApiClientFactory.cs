using Manager.Core.Networking;
using Microsoft.Extensions.Logging;

namespace Manager.WorkService.Client;

public interface IWorkServiceApiClientFactory
{
    IWorkServiceApiClient Create(string apiKey);
}

public class WorkServiceApiClientFactory(
    IPortProvider portProvider,
    ILogger<WorkServiceApiClient> logger,
    IResilientHttpClientFactory resilientHttpClientFactory
) : IWorkServiceApiClientFactory
{
    public IWorkServiceApiClient Create(string apiKey)
    {
        var port = portProvider.GetPort("WORK_SERVICE_PORT");
        var url = $"http://localhost:{port}";
        var httpClient = resilientHttpClientFactory.CreateClient(url, apiKey);
        return new WorkServiceApiClient(httpClient, logger);
    }
}