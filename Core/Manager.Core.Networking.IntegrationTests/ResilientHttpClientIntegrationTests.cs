using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.Networking.UnitTests; // Подключаем ваш MockHttpMessageHandler
using NUnit.Framework;

namespace Manager.Core.Networking.IntegrationTests;

public class ResilientHttpClientIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task SendAsync_ShouldSucceedWithMockedService()
    {
        var handler = new MockHttpMessageHandler((req, ct) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://fake-service.com/get");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task SendAsync_ShouldRetryAndFailWithMockedService()
    {
        var attempts = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                attempts++;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
            }
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(httpClient);
        var request = new HttpRequestMessage(HttpMethod.Get, "https://fake-service.com/status/503");

        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        attempts.Should().BeGreaterThan(1);
    }
}