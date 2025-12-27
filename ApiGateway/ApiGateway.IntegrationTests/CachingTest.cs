using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.Networking;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.ApiGateway.IntegrationTests;

public class CachingTest : IntegrationTestBase
{
    [Test]
    public async Task TestReadFromCacheNotExpiredValue()
    {
        // Arrange
        const string cacheKey = "testKey";
        var cacheValue = "testValue"u8.ToArray();

        // Act
        await DistributedCache.SetAsync(
            cacheKey,
            cacheValue,
            GetOptionsWithExpirationInSeconds(5)
        );

        // Asset
        var cacheResponse = await DistributedCache.GetAsync(cacheKey);
        cacheResponse.Should().NotBeNull();
        cacheResponse.Should().BeEquivalentTo(cacheValue);

        /*
         * Господь не любит программистов
         * Он никогда их не любил
         * Ошибок столько им насыпет
         * Что программистам свет не мил
         *
         * Но программисты не сдаются
         * Шагают гордо всё вперёд
         * Они ошибок не боятся
         * Надеется на них народ!
         *
         * И вот в борьбе ума и духа
         * Виднеется победы свет
         * Собрались с силой программисты
         * И запили в проект кэш
         */

        /*
         * Усталый взгляд и каша мыслей, 10 дней уже идёт
         * Непрерывная работа, лишь чтобы получить зачёт.
         * И 100 попыток совершили, но ни в одной не ждал успех
         * Контроллер мы не прикрутили, и не написали тест
         */

        /*
         * В своей любви к тебе признаюсь
         * В свои объятия заключу
         * И на руках носить я буду
         * Только работай блин, прошу 😭😭😭
         */
    }

    [Test]
    public async Task TestReadFromCacheExpiredValue()
    {
        // Arrange
        const string cacheKey = "testKey";
        var cacheValue = "testValue"u8.ToArray();

        // Act
        await DistributedCache.SetAsync(
            cacheKey,
            cacheValue,
            GetOptionsWithExpirationInSeconds(1)
        );

        // Asset
        await Task.Delay(TimeSpan.FromSeconds(2));
        var cacheResponse = await DistributedCache.GetAsync(cacheKey);
        cacheResponse.Should().BeNull();
    }

    [Test]
    [MaxTime(50)]
    public async Task TestSendHealthRequest_ShouldReturn200Ok()
    {
        // Arrange
        var httpClient = new HttpClient();

        var portProvider = ServiceProvider.GetRequiredService<IPortProvider>();
        var port = portProvider.GetPort("API_GATEWAY_PORT");
        var requestPath = $"http://localhost:{port}/health";
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, requestPath);

        // Act
        var httpResponseMessage = await httpClient.SendAsync(httpRequestMessage);

        // Assert
        httpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
        var stringContent = await httpResponseMessage.Content.ReadAsStringAsync();
        await TestContext.Out.WriteLineAsync(stringContent);
    }

    private static DistributedCacheEntryOptions GetOptionsWithExpirationInSeconds(int seconds) => new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(seconds),
    };
}