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
    public async Task SendAsync_WhenFallbackEnabled_ShouldFallbackAfterRetries()
    {
        var requestCount = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                requestCount++;
                throw new HttpRequestException("Network failure");
            }
        );

        var httpClient = new HttpClient(handler);
        var options = new HttpClientOptions { EnableFallback = true, RetryCount = 3, RetryDelayMs = 1 };

        var resilientHttpClient = new ResilientHttpClient(httpClient, options);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        requestCount.Should().Be(4); // 1 + 3 retry
    }

    [Test]
    public async Task SendAsync_WhenFallbackDisabled_ShouldThrowAfterRetries()
    {
        var requestCount = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                requestCount++;
                throw new HttpRequestException("Network failure");
            }
        );

        var httpClient = new HttpClient(handler);
        var options = new HttpClientOptions { EnableFallback = false, RetryCount = 3, RetryDelayMs = 1 };

        var resilientHttpClient = new ResilientHttpClient(httpClient, options);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        await resilientHttpClient
            .Invoking(x => x.SendAsync(request))
            .Should()
            .ThrowAsync<HttpRequestException>();

        requestCount.Should().Be(4);
    }
}