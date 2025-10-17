using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Manager.Core.Common.Linq;
using Manager.Core.CommonTestsCore;
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
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo()
        );
        createdWork.WorkStatus.Should().Be(WorkStatus.Actual);
    }

    [Test]
    public async Task TestUpdateWork()
    {
        // Arrange
        var workDbo = await CreateWorkAsync();

        var updateWorkRequest = Fixture.Build<UpdateWorkRequest>()
            .With(x => x.Id, workDbo.Id)
            .With(x => x.DeadLineUtc, Fixture.Create<DateTime>().ToUniversalTime())
            .Create();

        // Act
        await WorkServiceApiClient.UpdateWorkAsync(updateWorkRequest);

        // Asset
        var foundWorkDbo = await DataContext.FindAsync<WorkDbo, Guid>(workDbo.Id);
        foundWorkDbo.Should().BeEquivalentTo(
            updateWorkRequest,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo()
        );
        foundWorkDbo.RecipientId.Should().Be(workDbo.RecipientId);
        foundWorkDbo.WorkStatus.Should().Be(workDbo.WorkStatus);
        foundWorkDbo.CreatedUtc.Should().BeCloseTo(workDbo.CreatedUtc, TimeSpan.FromMilliseconds(10));
    }

    [Test]
    public async Task TestDeleteWork()
    {
        // Arrange
        var workDbo = await CreateWorkAsync();

        // Act
        await WorkServiceApiClient.DeleteWorkAsync(workDbo.Id);

        // Asset
        var foundWorkDbo = await DataContext.FindAsync<WorkDbo, Guid>(workDbo.Id);
        foundWorkDbo.Should().BeEquivalentTo(
            workDbo,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo().Excluding(x => x.WorkStatus)
        );
        foundWorkDbo.WorkStatus.Should().Be(WorkStatus.Deleted);
    }

    [Test]
    public async Task TestFindWork()
    {
        // Arrange
        var workDbo = await CreateWorkAsync();

        // Act
        var getWorkResponse = await WorkServiceApiClient.FindWorkAsync(workDbo.Id);

        // Assert
        getWorkResponse.Should().NotBeNull();
        getWorkResponse.Should().BeEquivalentTo(
            workDbo,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo().ExcludingMissingMembers()
        );
    }

    [Test]
    public async Task TestSelectWorks()
    {
        // Arrange
        var recipientId = Guid.NewGuid();
        var workDbos = await Enumerable.Repeat(CreateWorkAsync, 10)
            .SelectAsync(create => create(recipientId));

        // Act
        var getWorkResponses = await WorkServiceApiClient.SelectWorksAsync(recipientId);

        // Assert
        getWorkResponses.Should().HaveCount(workDbos.Length);
        getWorkResponses.Should().BeEquivalentTo(
            workDbos,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo().ExcludingMissingMembers()
        );
    }

    [Test]
    public async Task TestSelectActualWorks()
    {
        // Arrange
        var recipientId = Guid.NewGuid();
        var workDbos = await Enumerable.Repeat(CreateWorkAsync, 20)
            .SelectAsync(create => create(recipientId));
        var actualWorkDbos = workDbos.Where(x => x.WorkStatus == WorkStatus.Actual).ToArray();

        // Act
        var getWorkResponses = await WorkServiceApiClient.SelectActualWorksAsync(recipientId);

        // Assert
        getWorkResponses.Should().HaveCount(actualWorkDbos.Length);
        getWorkResponses.Should().BeEquivalentTo(
            actualWorkDbos,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo().ExcludingMissingMembers()
        );
    }

    [Test]
    public async Task TestSelectExpiredWorks()
    {
        // Arrange
        var recipientId = Guid.NewGuid();
        var workDbos = await Enumerable.Repeat(CreateWorkAsync, 20)
            .SelectAsync(create => create(recipientId));
        var actualWorkDbos = workDbos.Where(x => x.WorkStatus == WorkStatus.Expired).ToArray();

        // Act
        var getWorkResponses = await WorkServiceApiClient.SelectExpiredWorksAsync(recipientId);

        // Assert
        getWorkResponses.Should().HaveCount(actualWorkDbos.Length);
        getWorkResponses.Should().BeEquivalentTo(
            actualWorkDbos,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo().ExcludingMissingMembers()
        );
    }

    [Test]
    public async Task TestSelectDeletedWorks()
    {
        // Arrange
        var recipientId = Guid.NewGuid();
        var workDbos = await Enumerable.Repeat(CreateWorkAsync, 20)
            .SelectAsync(create => create(recipientId));
        var actualWorkDbos = workDbos.Where(x => x.WorkStatus == WorkStatus.Deleted).ToArray();

        // Act
        var getWorkResponses = await WorkServiceApiClient.SelectDeletedWorksAsync(recipientId);

        // Assert
        getWorkResponses.Should().HaveCount(actualWorkDbos.Length);
        getWorkResponses.Should().BeEquivalentTo(
            actualWorkDbos,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo().ExcludingMissingMembers()
        );
    }

    [Test]
    public async Task TestSelectCompletedWorks()
    {
        // Arrange
        var recipientId = Guid.NewGuid();
        var workDbos = await Enumerable.Repeat(CreateWorkAsync, 20)
            .SelectAsync(create => create(recipientId));
        var actualWorkDbos = workDbos.Where(x => x.WorkStatus == WorkStatus.Completed).ToArray();

        // Act
        var getWorkResponses = await WorkServiceApiClient.SelectCompletedWorksAsync(recipientId);

        // Assert
        getWorkResponses.Should().HaveCount(actualWorkDbos.Length);
        getWorkResponses.Should().BeEquivalentTo(
            actualWorkDbos,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo().ExcludingMissingMembers()
        );
    }

    [Test]
    [Ignore("У этого метода пока что нет реализации")]
    public Task TestSelectWorksForReminder()
    {
        return Task.CompletedTask;
    }

    private async Task<WorkDbo> CreateWorkAsync(Guid? recipientId = null)
    {
        var workDbo = Fixture.Build<WorkDbo>()
            .With(x => x.CreatedUtc, Fixture.Create<DateTime>().ToUniversalTime())
            .With(x => x.DeadLineUtc, Fixture.Create<DateTime>().ToUniversalTime())
            .With(x => x.RecipientId, recipientId ?? Guid.NewGuid())
            .Create();
        await DataContext.InsertAsync(workDbo);
        (await DataContext.FindAsync<WorkDbo, Guid>(workDbo.Id)).Should().BeEquivalentTo(
            workDbo,
            options => options.WithDateTimeCloseTo().WithTimeSpanCloseTo()
        );

        return workDbo;
    }
}