using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Manager.Core.Networking;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.ApiGateway.IntegrationalTests;

public class TimerClientTest : IntegrationTestBase
{
    [Test]
    public async Task TestApiGatewayCache()
    {
        // Arrange
        var client = new HttpClient();
        var portProvider = ServiceProvider.GetRequiredService<IPortProvider>();

        // Act
        var healthRequest = new HttpRequestMessage(
            HttpMethod.Get,
            $"http://localhost:{portProvider.GetPort("API_GATEWAY_PORT")}/Health"
        );
        var httpResponse = await client.SendAsync(healthRequest);

        // Asset
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var baseReturnedAnswer = await httpResponse.Content.ReadAsStringAsync();
        Thread.Sleep((int)(2 * 1e3));

        httpResponse = await client.SendAsync(await healthRequest.CloneAsync());
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var newReturnedAnswer = await httpResponse.Content.ReadAsStringAsync();
        newReturnedAnswer.Should().BeEquivalentTo(baseReturnedAnswer);
        Thread.Sleep((int)(40 * 1e3));

        httpResponse = await client.SendAsync(await healthRequest.CloneAsync());
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        newReturnedAnswer = await httpResponse.Content.ReadAsStringAsync();
        newReturnedAnswer.Should().NotBeEquivalentTo(baseReturnedAnswer);

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