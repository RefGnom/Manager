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
        var resilientHttpClient = new ResilientHttpClient(httpClient, enableFallback: true);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        var result = await resilientHttpClient.SendAsync(request);


        result.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        var content = await result.Content.ReadAsStringAsync();
        content.Should().Contain("Fallback response");

        requestCount.Should().Be(4);
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
        var resilientHttpClient = new ResilientHttpClient(httpClient, enableFallback: false);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        await resilientHttpClient
            .Invoking(x => x.SendAsync(request))
            .Should()
            .ThrowAsync<HttpRequestException>()
            .WithMessage("Network failure");

        requestCount.Should().Be(4);
    }

    [Test]
    public async Task SendAsync_WhenFallbackDisabled_ShouldReturnErrorStatusCode()
    {
        var requestCount = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                requestCount++;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.InternalServerError));
            }
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(httpClient, enableFallback: false);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://test");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        var content = await result.Content.ReadAsStringAsync();
        content.Should().NotContain("Fallback response");

        requestCount.Should().Be(4);
    }
}