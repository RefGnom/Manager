using System;
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
                var clonedRequest = await request.CloneAsync();
                return await httpClient.SendAsync(clonedRequest, ct);
            },
            cancellationToken
        );
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