using System;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace Manager.Core.Networking;

public class ResilientHttpClientFactory(
    IOptions<HttpClientOptions> options
) : IResilientHttpClientFactory
{
    public IHttpClient CreateClient(string url, string apiKey)
    {
        var currentOptions = options.Value;

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(url),
            DefaultRequestHeaders = { { "X-Api-Key", apiKey } },
        };

        if (currentOptions.TimeoutMs.HasValue)
        {
            httpClient.Timeout = TimeSpan.FromMilliseconds(currentOptions.TimeoutMs.Value);
        }

        return new ResilientHttpClient(httpClient, currentOptions);
    }
}