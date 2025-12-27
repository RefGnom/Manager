using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;

namespace Manager.Core.Networking.IntegrationTests;

public class ResilientHttpClientIntegrationTests : IntegrationTestBase
{
    [Test]
    public async Task Factory_WhenFallbackEnabled_ShouldReturn503()
    {
        var options = new HttpClientOptions
        {
            EnableFallback = true,
            RetryCount = 1,
            RetryDelayMs = 1,
            TimeoutMs = 100,
        };

        var factory = new ResilientHttpClientFactory(Options.Create(options));

        var client = factory.CreateClient("http://localhost:12345", "key");

        var result = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/"));

        result.StatusCode.Should().Be(HttpStatusCode.ServiceUnavailable);
        var content = await result.Content.ReadAsStringAsync();
        content.Should().Contain("Fallback response");
    }

    [Test]
    public async Task Factory_WhenFallbackDisabled_ShouldThrowException()
    {
        var options = new HttpClientOptions
        {
            EnableFallback = false,
            RetryCount = 1,
            RetryDelayMs = 1,
            TimeoutMs = 100,
        };

        var factory = new ResilientHttpClientFactory(Options.Create(options));
        var client = factory.CreateClient("http://localhost:12345", "key");

        await client
            .Invoking(x => x.SendAsync(new HttpRequestMessage(HttpMethod.Get, "/")))
            .Should()
            .ThrowAsync<Exception>();
    }
}