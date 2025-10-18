using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.Core.CommonTestsCore;
using Manager.Core.IntegrationTestsCore;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.Layers.DbLayer.Dbos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Manager.TimerService.IntegrationTests;

public class TimerClientTest : IntegrationTestBase
{
    private ITimerServiceApiClient TimerServiceApiClient =>
        ServiceProvider.GetRequiredService<ITimerServiceApiClient>();

    [Test]
    public async Task TestCreateWork()
    {
        // Arrange
        var startTimerRequest = Fixture.Build<StartTimerRequest>()
            //.With(x => x.StartTime, Fixture.Create<DateTime>().ToUniversalTime())
            .Create();

        // Act
        var httpResponse = await TimerServiceApiClient.StartTimerAsync(startTimerRequest);
        TestContext.WriteLine(httpResponse.ResponseMessage);

        // Asset
        httpResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        var createdTimer = await DataContext.ExecuteReadAsync<TimerDbo, TimerDbo>(query =>
            query.Where(x => x.Name == startTimerRequest.Name)
                .Where(x => x.UserId == startTimerRequest.UserId)
                .FirstAsync()
        );
        createdTimer.Should().NotBeNull();
        createdTimer.Should().BeEquivalentTo(
            startTimerRequest,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo()
        );
        createdTimer.Status.Should().Be(TimerStatus.Started);
    }
}