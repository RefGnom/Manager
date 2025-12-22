using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.Networking;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.ApiGateway.IntegrationalTests;

public class TimerClientTest : IntegrationTestBase
{
    [Test]
    public async Task TestApiGatewayCache()
    {
        // Arrange
        var cache = ServiceProvider.GetRequiredService<IDistributedCache>();
        const string cacheKey = "testKey";
        var cacheValue = "testValue".Select(x => (byte)x).ToArray();
        // Act
        cache.Should().NotBeNull("Низя");
        await cache.SetAsync(cacheKey, cacheValue);

        // Asset
        var cacheResponse = await cache.GetAsync(cacheKey);
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
    }
}