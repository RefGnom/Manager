using System;
using System.Net.Http;
using Microsoft.Extensions.Configuration;

namespace Manager.Core.Networking;

public class ResilientHttpClientFactory(
    IConfiguration configuration
) : IResilientHttpClientFactory
{
    public IHttpClient CreateClient(string url, string apiKey)
    {
        var section = configuration.GetSection("HttpClientOptions");

        var enableFallback = section.GetValue<bool>("EnableFallback"); // false
        var retryCount = section.GetValue<int?>("RetryCount") ?? 3;

        var retryDelayMs = section.GetValue<int?>("RetryDelayMs");
        TimeSpan? fixedDelay = retryDelayMs.HasValue ? TimeSpan.FromMilliseconds(retryDelayMs.Value) : null;

        var timeoutMs = section.GetValue<int?>("TimeoutMs");

        var httpClient = new HttpClient
        {
            BaseAddress = new Uri(url),
            DefaultRequestHeaders = { { "X-Api-Key", apiKey } },
        };

        if (timeoutMs.HasValue)
        {
            httpClient.Timeout = TimeSpan.FromMilliseconds(timeoutMs.Value);
        }

        return new ResilientHttpClient(httpClient, enableFallback, retryCount, fixedDelay);
    }
}