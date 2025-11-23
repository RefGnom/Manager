using System;
using System.Net.Http;

namespace Manager.Core.Networking;

public interface IResilientHttpClientFactory
{
    IHttpClient CreateClient(string url, string apiKey);
}

public class ResilientHttpClientFactory : IResilientHttpClientFactory
{
    public IHttpClient CreateClient(string url, string apiKey)
    {
        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(url),
            DefaultRequestHeaders = { { "X-Api-Key", apiKey } },
        };
        return new ResilientHttpClient(httpClient);
    }
}