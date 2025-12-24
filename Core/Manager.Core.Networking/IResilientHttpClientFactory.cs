namespace Manager.Core.Networking;

public interface IResilientHttpClientFactory
{
    IHttpClient CreateClient(string url, string apiKey);
}