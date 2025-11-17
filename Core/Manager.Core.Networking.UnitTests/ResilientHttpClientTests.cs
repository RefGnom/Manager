using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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

        var result = await resilientHttpClient.SendAsync(request, CancellationToken.None);

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        Assert.That(requestCount, Is.EqualTo(2));
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

        var result = await resilientHttpClient.SendAsync(request, CancellationToken.None);

        Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        Assert.That(requestCount, Is.EqualTo(4));
    }
}