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
    private readonly AsyncPolicyWrap<HttpResponseMessage> policyWrap;
    private readonly HttpClient httpClient;

    public ResilientHttpClient(HttpClient httpClient)
    {
        this.httpClient = httpClient;
        var retryPolicy = HttpPolicyBuilders.GetRetryPolicy();
        var fallbackPolicy = HttpPolicyBuilders.GetFallbackPolicy();

        policyWrap = Policy.WrapAsync(fallbackPolicy, retryPolicy);
    }

    public Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default
    )
    {
        return policyWrap.ExecuteAsync(
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
    public static AsyncRetryPolicy<HttpResponseMessage> GetRetryPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r =>
                (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout
            )
            .Or<HttpRequestException>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }

    public static AsyncFallbackPolicy<HttpResponseMessage> GetFallbackPolicy()
    {
        return Policy
            .HandleResult<HttpResponseMessage>(r =>
                (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout
            )
            .Or<HttpRequestException>()
            .FallbackAsync(
                new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent("Fallback response"),
                }
            );
    }
}