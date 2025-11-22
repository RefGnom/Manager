using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using NUnit.Framework;

namespace Manager.Core.Networking.IntegrationTests;

public class ResilientHttpClientIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task SendAsync_ShouldSucceedWithRealService()
    {
        var httpClient = new HttpClient();
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://httpbin.org/get");

        var result = await resilientHttpClient.SendAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task SendAsync_ShouldRetryAndFailForRealService()
    {
        var httpClient = new HttpClient();
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://httpbin.org/status/503");

        var result = await resilientHttpClient.SendAsync(request, CancellationToken.None);

        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }
}