using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.Fallback;

namespace Manager.Core.Networking;

public class ResilientHttpClient : IHttpClient
{
    private readonly IAsyncPolicy<HttpResponseMessage> resiliencePipeline;
    private readonly HttpClient httpClient;

    public ResilientHttpClient(
        HttpClient httpClient,
        HttpClientOptions options
    )
    {
        this.httpClient = httpClient;

        TimeSpan? fixedDelay = options.RetryDelayMs.HasValue
            ? TimeSpan.FromMilliseconds(options.RetryDelayMs.Value)
            : null;

        IAsyncPolicy<HttpResponseMessage> policy = HttpPolicyBuilders.GetRetryPolicy(options.RetryCount, fixedDelay);

        if (options.EnableFallback)
        {
            var fallbackPolicy = HttpPolicyBuilders.GetFallbackPolicy();
            policy = Policy.WrapAsync(fallbackPolicy, policy);
        }

        resiliencePipeline = policy;
    }

    public Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default
    )
    {
        return resiliencePipeline.ExecuteAsync(
            async ct =>
            {
                var clonedRequest = await CloneHttpRequestMessageAsync(request);
                return await httpClient.SendAsync(clonedRequest, ct);
            },
            cancellationToken
        );
    }

    private static async Task<HttpRequestMessage> CloneHttpRequestMessageAsync(HttpRequestMessage req)
    {
        var clone = new HttpRequestMessage(req.Method, req.RequestUri);

        if (req.Content != null)
        {
            var ms = new System.IO.MemoryStream();
            await req.Content.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);

            if (req.Content.Headers != null)
                foreach (var h in req.Content.Headers)
                    clone.Content.Headers.Add(h.Key, h.Value);
        }

        clone.Version = req.Version;

        foreach (var prop in req.Options)
            clone.Options.TryAdd(prop.Key, prop.Value);

        foreach (var header in req.Headers)
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);

        return clone;
    }
}

public static class HttpPolicyBuilders
{
    public static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy(int retryCount, TimeSpan? fixedDelay)
    {
        var policyBuilder = Policy
            .HandleResult<HttpResponseMessage>(r =>
                (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout
            )
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>();

        return fixedDelay.HasValue
            ? policyBuilder.WaitAndRetryAsync(retryCount, _ => fixedDelay.Value)
            : policyBuilder.WaitAndRetryAsync(
                retryCount,
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))
            );
    }

    public static AsyncFallbackPolicy<HttpResponseMessage> GetFallbackPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r =>
                (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout
            )
            .Or<HttpRequestException>()
            .Or<TaskCanceledException>()
            .FallbackAsync(
                new HttpResponseMessage(HttpStatusCode.ServiceUnavailable)
                {
                    Content = new StringContent("Fallback response: Service is currently unavailable."),
                }
            );
    }
}