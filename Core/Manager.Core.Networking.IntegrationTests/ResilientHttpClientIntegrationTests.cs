using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.Networking.UnitTests;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;

namespace Manager.Core.Networking.IntegrationTests;

public class ResilientHttpClientIntegrationTests : IntegrationTestBase
{
    private readonly Dictionary<string, string?> _fastTestSettings = new()
    {
        { "HttpClientOptions:RetryDelayMs", "10" },
        { "HttpClientOptions:TimeoutMs", "1000" },
    };

    [Test]
    public async Task Manual_SendAsync_WhenServiceIsHealthy_ShouldSucceed()
    {
        var handler = new MockHttpMessageHandler((req, ct) =>
            Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK))
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(
            httpClient,
            enableFallback: false,
            retryCount: 1,
            fixedRetryDelay: TimeSpan.FromMilliseconds(1)
        );

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/test");
        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Test]
    public async Task Manual_SendAsync_ShouldRetryAndReturnError_WhenFallbackDisabled()
    {
        var attempts = 0;
        var handler = new MockHttpMessageHandler((req, ct) =>
            {
                attempts++;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.ServiceUnavailable));
            }
        );

        var httpClient = new HttpClient(handler);
        var resilientHttpClient = new ResilientHttpClient(
            httpClient,
            enableFallback: false,
            retryCount: 1,
            fixedRetryDelay: TimeSpan.FromMilliseconds(1)
        );

        var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost/api/error");
        var result = await resilientHttpClient.SendAsync(request);

        result.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        attempts.Should().Be(2); // 1 попытка + 1 ретрай
    }

    [Test]
    public async Task Scenario_ServerFalse_ClientTrue_ShouldUseFallback()
    {
        var configDict = new Dictionary<string, string?>(_fastTestSettings)
        {
            { "EnableFallback", "false" },
            { "HttpClientOptions:EnableFallback", "true" },
        };

        var factory = CreateFactoryWithConfig(configDict);

        var client = factory.CreateClient("http://localhost:12345", "key");

        var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"));

        result.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        var content = await result.Content.ReadAsStringAsync();
        content.Should().Contain("Fallback response");
    }

    [Test]
    public async Task Scenario_ServerTrue_ClientFalse_ShouldThrowException()
    {
        var configDict = new Dictionary<string, string?>(_fastTestSettings)
        {
            { "EnableFallback", "true" },
            { "HttpClientOptions:EnableFallback", "false" },
        };

        var factory = CreateFactoryWithConfig(configDict);
        var client = factory.CreateClient("http://localhost:12345", "key");

        await client
            .Invoking(x => x.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/")))
            .Should()
            .ThrowAsync<Exception>(); // Может быть HttpRequestException или TaskCanceledException (из-за таймаута)
    }

    [Test]
    public async Task Scenario_Defaults_ShouldThrowException()
    {
        var configDict = new Dictionary<string, string?>(_fastTestSettings);

        var factory = CreateFactoryWithConfig(configDict);
        var client = factory.CreateClient("http://localhost:12345", "key");

        await client
            .Invoking(x => x.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/")))
            .Should()
            .ThrowAsync<Exception>();
    }

    private IResilientHttpClientFactory CreateFactoryWithConfig(Dictionary<string, string?> settings)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();
        return new ResilientHttpClientFactory(configuration);
    }
}