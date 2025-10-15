using System;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.Core.IntegrationTestsCore;
using Manager.WorkService.Client;
using Manager.WorkService.Client.Requests;
using Manager.WorkService.Server.Layers.Repository.Dbos;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using WorkStatus = Manager.WorkService.Server.Layers.BusinessLogic.Models.WorkStatus;

namespace Manager.WorkService.IntegrationTests;

public class WorkClientTest : IntegrationTestBase
{
    private IWorkServiceApiClient WorkServiceApiClient => ServiceProvider.GetRequiredService<IWorkServiceApiClient>();

    [Test]
    public async Task TestCreateWork()
    {
        // Arrange
        var createWorkRequest = Fixture.Build<CreateWorkRequest>()
            .With(x => x.DeadLineUtc, Fixture.Create<DateTime>().ToUniversalTime())
            .Create();

        // Act
        var workId = await WorkServiceApiClient.CreateWorkAsync(createWorkRequest);

        // Asset
        var createdWork = await DataContext.FindAsync<WorkDbo, Guid>(workId);
        createdWork.Should().NotBeNull();
        createdWork.Should().BeEquivalentTo(
            createWorkRequest,
            options => options
                .Using<DateTime>(x => x.Expectation.Should().BeCloseTo(x.Subject, TimeSpan.FromMilliseconds(100)))
                .WhenTypeIs<DateTime>()
                .Using<TimeSpan>(x => x.Expectation.Should().BeCloseTo(x.Subject, TimeSpan.FromMilliseconds(100)))
                .WhenTypeIs<TimeSpan>()
        );
        createdWork.WorkStatus.Should().Be(WorkStatus.Actual);
    }
}