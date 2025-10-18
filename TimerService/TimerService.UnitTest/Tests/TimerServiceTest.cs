using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.Common.Time;
using Manager.Core.UnitTestsCore;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;
using Manager.TimerService.Server.Layers.ServiceLayer.Factories;
using Manager.TimerService.Server.Layers.ServiceLayer.Services;
using Manager.TimerService.Server.ServiceModels;
using Manager.TimerService.UnitTest.Factories;
using Manager.TimerService.UnitTest.Factories.Extensions;
using Manager.TimerService.UnitTest.MockSetupHelpers;
using NSubstitute;
using NUnit.Framework;

namespace Manager.TimerService.UnitTest.Tests;

public class TimerServiceTest : UnitTestBase
{
    private static readonly TimerDtoTestFactory timerFactory = new();
    private static readonly TimerSessionDtoTestFactory sessionFactory = new();
    private IDateTimeProvider dateTimeProvider = null!;
    private ITimerDtoFactory timerDtoFactory = null!;

    private ITimerRepository timerRepository = null!;
    private ITimerSessionService timerSessionService = null!;
    private TimersService timersService = null!;

    [SetUp]
    public void Setup()
    {
        timerRepository = Substitute.For<ITimerRepository>();
        timerSessionService = Substitute.For<ITimerSessionService>();
        dateTimeProvider = Substitute.For<IDateTimeProvider>();
        timerDtoFactory = Substitute.For<ITimerDtoFactory>();
        timersService = new TimersService(
            timerRepository,
            timerSessionService,
            dateTimeProvider,
            timerDtoFactory
        );
    }

    [Test]
    public async Task StartCreatesAndStartsSessionCorrect()
    {
        var timer = timerFactory.CreateEmptyTimer();
        await timersService.StartAsync(timer);

        await timerRepository
            .Received()
            .CreateAsync(
                Arg.Is<TimerDto>(x => x.Status == TimerStatus.Started
                )
            );

        await timerSessionService
            .Received()
            .StartAsync(timer.Id, timer.StartTime!.Value);
    }

    [Test]
    public async Task StartTimerWithInvalidStatusThrowsException()
    {
        var timer = timerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Started);

        timerRepository.ConfigureFindMethod(timer);

        await timersService
            .Invoking(x => x.StartAsync(timer))
            .Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task StartStoppedTimerCorrect()
    {
        var timer = timerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Stopped);
        timerRepository.ConfigureFindMethod(timer);

        await timersService.StartAsync(timer);

        await timerRepository
            .Received()
            .UpdateAsync(Arg.Is<TimerDto>(x => x.Status == TimerStatus.Started));

        await timerSessionService
            .Received()
            .StartAsync(timer.Id, timer.StartTime!.Value);
    }

    public static IEnumerable<TestCaseData> GetSelectByUserFilterTestCases()
    {
        yield return new TestCaseData(
            new[]
            {
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Started),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Stopped),
            },
            false,
            false
        );

        yield return new TestCaseData(
            new[]
            {
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Started),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Stopped),
            },
            true,
            false
        );

        yield return new TestCaseData(
            new[]
            {
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Started),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
                timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted),
            },
            true,
            true
        );
    }

    [TestCaseSource(nameof(GetSelectByUserFilterTestCases))]
    [Test]
    public async Task SelectByUser_FiltersCorrect(TimerDto[] resultTimers, bool withArchived, bool withDeleted)
    {
        var userId = Guid.NewGuid();
        timerRepository.ConfigureSelectByUserMethod(userId, resultTimers);
        var timers = await timersService.SelectByUserAsync(userId, withArchived, withDeleted);

        if (!withArchived)
        {
            timers
                .Should()
                .NotContain(x => x.Status == TimerStatus.Archived);
        }

        if (!withDeleted)
        {
            timers
                .Should()
                .NotContain(x => x.Status == TimerStatus.Deleted);
        }
    }

    [Test]
    public async Task StopValidTimerBeCorrect()
    {
        var timer = timerFactory.CreateEmptyTimer()
            .WithStatus(TimerStatus.Started)
            .WithSessions([sessionFactory.CreateEmptySession()]);
        timerRepository.ConfigureFindMethod(timer);

        await timersService.StopAsync(timer.UserId, timer.Name, DateTime.MinValue);

        timer.Status
            .Should()
            .Be(TimerStatus.Stopped);

        await timerSessionService
            .Received()
            .StopTimerSessionAsync(timer.Id, DateTime.MinValue);

        await timerRepository
            .Received()
            .UpdateAsync(timer);
    }

    [Test]
    public async Task StopTimerWithInvalidStatusThrowsException()
    {
        var timer = timerFactory.CreateEmptyTimer();
        timerRepository.ConfigureFindMethod(timer);

        await timersService.Invoking(x =>
                x.StopAsync(timer.UserId, timer.Name, DateTime.MinValue)
            ).Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task StopNotExistedTimerThrowsException()
    {
        var timer = timerFactory.CreateEmptyTimer();
        timerRepository.ConfigureFindMethod(timer.UserId, timer.Name, null);

        await timersService.Invoking(x =>
                x.StopAsync(timer.UserId, timer.Name, DateTime.MinValue)
            ).Should()
            .ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task FindTimerCorrect()
    {
        var timer = timerFactory.CreateEmptyTimer();
        timerRepository.ConfigureFindMethod(timer);

        var result = await timersService.FindAsync(timer.UserId, timer.Name);

        result
            .Should()
            .BeEquivalentTo(timer);
    }

    public async Task FindNotExistedTimerThrowsException()
    {
        var timer = timerFactory.CreateEmptyTimer();
        timerRepository.ConfigureFindMethod(timer.UserId, timer.Name, null);

        await timersService.Invoking(x =>
                x.FindAsync(timer.UserId, timer.Name)
            ).Should()
            .ThrowAsync<NotFoundException>();
    }

    [Test]
    public async Task ResetTimerCorrect()
    {
        var timer = timerFactory.CreateEmptyTimer()
            .WithStatus(TimerStatus.Stopped)
            .WithSessions([sessionFactory.CreateEmptySession()]);

        timerRepository.ConfigureFindMethod(timer);

        var archivedTimer = timerFactory
            .CreateSameTimer(timer)
            .WithName("archived");

        timerDtoFactory.ConfigureCreateArchivedTimer(timer, archivedTimer);

        var resetTimer = timerFactory.CreateSameTimer(timer).WithNewId();
        timerDtoFactory.ConfigureCreateResetTimer(timer, resetTimer);

        await timersService.ResetAsync(timer.UserId, timer.Name);

        await timerRepository
            .Received()
            .UpdateAsync(
                Arg.Is<TimerDto>(x =>
                    x.Id == timer.Id && x.Name.Contains("archived")
                )
            );

        await timerRepository
            .Received()
            .CreateAsync(Arg.Is<TimerDto>(x => x.Name == timer.Name && x.UserId == timer.UserId));
    }

    [Test]
    public async Task ResetTimerWithInvalidStatusThrowsException()
    {
        var timer = timerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Started);

        timerRepository.ConfigureFindMethod(timer);
        await timersService.Invoking(x => x.ResetAsync(timer.UserId, timer.Name)
            ).Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task DeleteTimerCorrect()
    {
        var timer = timerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Stopped);
        timerRepository.ConfigureFindMethod(timer);

        var deletedTimer = timerFactory
            .CreateSameTimer(timer)
            .WithName("deleted");

        timerDtoFactory.ConfigureCreateDeletedTimer(timer, deletedTimer);

        await timersService.DeleteAsync(timer.UserId, timer.Name);

        await timerRepository
            .Received()
            .UpdateAsync(
                Arg.Is<TimerDto>(x =>
                    x.Name.Contains("deleted")
                )
            );
    }

    [Test]
    public async Task DeleteTimerWithInvalidStatusThrowsException()
    {
        var timer = timerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Started);

        timerRepository.ConfigureFindMethod(timer);

        await timersService.Invoking(x =>
                x.DeleteAsync(timer.Id, timer.Name)
            ).Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task DeleteNotExistedTimerThrowsException()
    {
        var timer = timerFactory.CreateEmptyTimer();
        timerRepository.ConfigureFindMethod(timer.UserId, timer.Name, null);

        await timersService.Invoking(x =>
                x.DeleteAsync(timer.UserId, timer.Name)
            ).Should()
            .ThrowAsync<NotFoundException>();
    }

    public static IEnumerable<TestCaseData> GetCalculateElapsedTimeCorrectTestCases()
    {
        yield return new TestCaseData(
            new[]
            {
                sessionFactory.CreateFromTimes(
                    DateTime.UtcNow - TimeSpan.FromHours(5),
                    DateTime.UtcNow - TimeSpan.FromHours(1)
                ),
            },
            TimeSpan.FromHours(4)
        );

        yield return new TestCaseData(
            new[]
            {
                sessionFactory.CreateFromTimes(
                    DateTime.UtcNow - TimeSpan.FromHours(18),
                    DateTime.UtcNow - TimeSpan.FromHours(15)
                ),
                sessionFactory.CreateFromTimes(
                    DateTime.UtcNow - TimeSpan.FromHours(5),
                    DateTime.UtcNow - TimeSpan.FromHours(1)
                ),
            },
            TimeSpan.FromHours(7)
        );

        yield return new TestCaseData(
            new[]
            {
                sessionFactory.CreateFromTimes(
                    DateTime.UtcNow - TimeSpan.FromHours(8),
                    DateTime.UtcNow - TimeSpan.FromHours(5)
                ),
                sessionFactory.CreateFromTimes(DateTime.UtcNow - TimeSpan.FromHours(2), null),
            },
            TimeSpan.FromHours(5)
        );
    }

    [Test]
    [TestCaseSource(nameof(GetCalculateElapsedTimeCorrectTestCases))]
    public void CalculateElapsedTimeSessionsCorrect(TimerSessionDto[] sessions, TimeSpan expectedElapsedTime)
    {
        var timer = timerFactory
            .CreateEmptyTimer()
            .WithSessions(sessions);
        dateTimeProvider.UtcNow
            .Returns(DateTime.UtcNow);
        var result = timersService.CalculateElapsedTime(timer);
        result
            .Should()
            .BeCloseTo(expectedElapsedTime, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void CalculateElapsedTimeWithEmptySessionsReturnsZero()
    {
        var timer = timerFactory
            .CreateEmptyTimer()
            .WithSessions([]);

        var result = timersService.CalculateElapsedTime(timer);
        result
            .Should()
            .Be(TimeSpan.Zero);
    }
}