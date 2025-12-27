using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;
using Polly.Retry;
using Polly.Fallback;
using Polly.Wrap;

namespace Manager.Core.Networking;

public class ResilientHttpClient : IHttpClient
{
    private readonly AsyncPolicyWrap<HttpResponseMessage>? policyWrap;
    private readonly AsyncRetryPolicy<HttpResponseMessage> retryPolicy;
    private readonly HttpClient httpClient;
    private readonly bool enableFallback;

    public ResilientHttpClient(
        HttpClient httpClient,
        bool enableFallback,
        int retryCount = 3,
        TimeSpan? fixedRetryDelay = null
    )
    {
        this.httpClient = httpClient;
        this.enableFallback = enableFallback;

        retryPolicy = HttpPolicyBuilders.GetRetryPolicy(retryCount, fixedRetryDelay);

        if (!enableFallback)
        {
            return;
        }

        var fallbackPolicy = HttpPolicyBuilders.GetFallbackPolicy();
        policyWrap = Policy.WrapAsync(fallbackPolicy, retryPolicy);
    }

    public Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default
    )
    {
        if (enableFallback && policyWrap != null)
        {
            return policyWrap.ExecuteAsync(
                async ct =>
                {
                    var cloned = await CloneHttpRequestMessageAsync(request);
                    return await httpClient.SendAsync(cloned, ct);
                },
                cancellationToken
            );
        }

        return retryPolicy.ExecuteAsync(
            async ct =>
            {
                var cloned = await CloneHttpRequestMessageAsync(request);
                return await httpClient.SendAsync(cloned, ct);
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
            foreach (var h in req.Content.Headers)
                clone.Content.Headers.Add(h.Key, h.Value);
        }

        clone.Version = req.Version;
        foreach (var prop in req.Options) clone.Options.TryAdd(prop.Key, prop.Value);
        foreach (var header in req.Headers) clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
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
        ;

        // Если задержка задана, используем её вместо экспоненты
        if (fixedDelay.HasValue)
        {
            return policyBuilder.WaitAndRetryAsync(retryCount, _ => fixedDelay.Value);
        }

        return policyBuilder.WaitAndRetryAsync(
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