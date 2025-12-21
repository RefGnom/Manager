using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.UnitTestsCore;
using NUnit.Framework;

namespace Manager.Core.Networking.UnitTests;

public class ResilientHttpClientTests : UnitTestBase
{
    [Test]
    public async Task SendAsync_ShouldRetryThenSucceed()
    {
        var requestCount = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                requestCount++;
                return requestCount == 1
                    ? throw new HttpRequestException()
                    : Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(2);
    }

    [Test]
    public async Task SendAsync_ShouldFallbackAfterRetries()
    {
        var requestCount = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                requestCount++;
                throw new HttpRequestException();
            }
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var content = await result.Content.ReadAsStringAsync();
        content.Should().Be("Fallback response");
        requestCount.Should().Be(4);
    }

    [Test]
    public async Task SendAsync_ShouldNotRetry_OnClientError()
    {
        var requestCount = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                requestCount++;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.BadRequest));
            }
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        requestCount.Should().Be(1);
    }

    [Test]
    public async Task SendAsync_ShouldRetry_OnServerError()
    {
        var requestCount = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                requestCount++;
                return requestCount == 1
                    ? Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError))
                    : Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            }
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
        requestCount.Should().Be(2);
    }
}