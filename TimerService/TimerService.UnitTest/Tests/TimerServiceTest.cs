using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.DateTimeProvider;
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

[TestFixture]
public class TimerServiceTest
{
    private static readonly ITimerDtoTestFactory TimerFactory = new TimerDtoTestFactory();
    private static readonly ITimerSessionDtoTestFactory SessionFactory = new TimerSessionDtoTestFactory();

    private ITimerRepository _timerRepository;
    private ITimerSessionService _timerSessionService;
    private IDateTimeProvider _dateTimeProvider;
    private ITimerDtoFactory _timerDtoFactory;
    private TimersService _timersService;

    [SetUp]
    public void Setup()
    {
        _timerRepository = Substitute.For<ITimerRepository>();
        _timerSessionService = Substitute.For<ITimerSessionService>();
        _dateTimeProvider = Substitute.For<IDateTimeProvider>();
        _timerDtoFactory = Substitute.For<ITimerDtoFactory>();
        _timersService = new TimersService(
            _timerRepository,
            _timerSessionService,
            _dateTimeProvider,
            _timerDtoFactory
        );
    }

    #region Start

    [Test]
    public async Task StartCreatesAndStartsSessionCorrect()
    {
        var timer = TimerFactory.CreateEmptyTimer();
        await _timersService.StartAsync(timer);

        await _timerRepository
            .Received()
            .CreateAsync(Arg.Is<TimerDto>(
                x => x.Status == TimerStatus.Started
            ));

        await _timerSessionService
            .Received()
            .StartAsync(timer.Id, timer.StartTime!.Value);
    }

    [Test]
    public async Task StartTimerWithInvalidStatusThrowsException()
    {
        var timer = TimerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Started);

        _timerRepository.ConfigureFindMethod(timer);

        await _timersService
            .Invoking(x => x.StartAsync(timer))
            .Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task StartStoppedTimerCorrect()
    {
        var timer = TimerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Stopped);
        _timerRepository.ConfigureFindMethod(timer);

        await _timersService.StartAsync(timer);

        await _timerRepository
            .Received()
            .UpdateAsync(Arg.Is<TimerDto>(
                x => x.Status == TimerStatus.Started));

        await _timerSessionService
            .Received()
            .StartAsync(timer.Id, timer.StartTime!.Value);
    }

    #endregion

    #region SelectByUser

    #region TestCases

    public static IEnumerable<TestCaseData> GetSelectByUserFilterTestCases()
    {
        yield return new TestCaseData(new[]
        {
            TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Started),
            TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted),
            TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted),
            TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
            TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Stopped)
        }, false, false);

        yield return new TestCaseData(
            new[]
            {
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Started),
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted),
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Stopped)
            },
            true,
            false
        );

        yield return new TestCaseData(
            new[]
            {
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Started),
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Archived),
                TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Deleted)
            },
            true,
            true
        );
    }

    #endregion

    [TestCaseSource(nameof(GetSelectByUserFilterTestCases))]
    [Test]
    public async Task SelectByUser_FiltersCorrect(TimerDto[] resultTimers, bool withArchived, bool withDeleted)
    {
        var userId = Guid.NewGuid();
        _timerRepository.ConfigureSelectByUserMethod(userId, resultTimers);
        var timers = await _timersService.SelectByUserAsync(userId, withArchived, withDeleted);

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

    #endregion

    #region Stop

    [Test]
    public async Task StopValidTimerBeCorrect()
    {
        var timer = TimerFactory.CreateEmptyTimer()
            .WithStatus(TimerStatus.Started)
            .WithSessions([SessionFactory.CreateEmptySession()]);
        _timerRepository.ConfigureFindMethod(timer);

        await _timersService.StopAsync(timer.UserId, timer.Name, DateTime.MinValue);

        timer.Status
            .Should()
            .Be(TimerStatus.Stopped);

        await _timerSessionService
            .Received()
            .StopTimerSessionAsync(timer.Id, DateTime.MinValue);

        await _timerRepository
            .Received()
            .UpdateAsync(timer);
    }

    [Test]
    public async Task StopTimerWithInvalidStatusThrowsException()
    {
        var timer = TimerFactory.CreateEmptyTimer();
        _timerRepository.ConfigureFindMethod(timer);

        await _timersService.Invoking(x =>
                x.StopAsync(timer.UserId, timer.Name, DateTime.MinValue)
            ).Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task StopNotExistedTimerThrowsException()
    {
        var timer = TimerFactory.CreateEmptyTimer();
        _timerRepository.ConfigureFindMethod(timer.UserId, timer.Name, null);

        await _timersService.Invoking(x =>
                x.StopAsync(timer.UserId, timer.Name, DateTime.MinValue)
            ).Should()
            .ThrowAsync<NotFoundException>();
    }

    #endregion

    #region Find

    [Test]
    public async Task FindTimerCorrect()
    {
        var timer = TimerFactory.CreateEmptyTimer();
        _timerRepository.ConfigureFindMethod(timer);

        var result = await _timersService.FindAsync(timer.UserId, timer.Name);

        result
            .Should()
            .BeEquivalentTo(timer);
    }

    public async Task FindNotExistedTimerThrowsException()
    {
        var timer = TimerFactory.CreateEmptyTimer();
        _timerRepository.ConfigureFindMethod(timer.UserId, timer.Name, null);

        await _timersService.Invoking(x =>
                x.FindAsync(timer.UserId, timer.Name)
            ).Should()
            .ThrowAsync<NotFoundException>();
    }

    #endregion

    #region Reset

    [Test]
    public async Task ResetTimerCorrect()
    {
        var timer = TimerFactory.CreateEmptyTimer()
            .WithStatus(TimerStatus.Stopped)
            .WithSessions([SessionFactory.CreateEmptySession()]);

        _timerRepository.ConfigureFindMethod(timer);

        var archivedTimer = TimerFactory
            .CreateSameTimer(timer)
            .WithName("archived");

        _timerDtoFactory.ConfigureCreateArchivedTimer(timer, archivedTimer);

        var resetTimer = TimerFactory.CreateSameTimer(timer).WithNewId();
        _timerDtoFactory.ConfigureCreateResetTimer(timer, resetTimer);

        await _timersService.ResetAsync(timer.UserId, timer.Name);

        await _timerRepository
            .Received()
            .UpdateAsync(Arg.Is<TimerDto>(x =>
                x.Id == timer.Id && x.Name.Contains("archived"))
            );

        await _timerRepository
            .Received()
            .CreateAsync(Arg.Is<TimerDto>(
                x => x.Name == timer.Name && x.UserId == timer.UserId));
    }

    [Test]
    public async Task ResetTimerWithInvalidStatusThrowsException()
    {
        var timer = TimerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Started);

        _timerRepository.ConfigureFindMethod(timer);
        await _timersService.Invoking(
                x => x.ResetAsync(timer.UserId, timer.Name)
            ).Should()
            .ThrowAsync<InvalidStatusException>();
    }

    #endregion

    #region Delete

    [Test]
    public async Task DeleteTimerCorrect()
    {
        var timer = TimerFactory.CreateEmptyTimer().WithStatus(TimerStatus.Stopped);
        _timerRepository.ConfigureFindMethod(timer);

        var deletedTimer = TimerFactory
            .CreateSameTimer(timer)
            .WithName("deleted");

        _timerDtoFactory.ConfigureCreateDeletedTimer(timer, deletedTimer);

        await _timersService.DeleteAsync(timer.UserId, timer.Name);

        await _timerRepository
            .Received()
            .UpdateAsync(Arg.Is<TimerDto>(x =>
                x.Name.Contains("deleted"))
            );
    }

    [Test]
    public async Task DeleteTimerWithInvalidStatusThrowsException()
    {
        var timer = TimerFactory
            .CreateEmptyTimer()
            .WithStatus(TimerStatus.Started);

        _timerRepository.ConfigureFindMethod(timer);

        await _timersService.Invoking(x =>
                x.DeleteAsync(timer.Id, timer.Name)
            ).Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task DeleteNotExistedTimerThrowsException()
    {
        var timer = TimerFactory.CreateEmptyTimer();
        _timerRepository.ConfigureFindMethod(timer.UserId, timer.Name, null);

        await _timersService.Invoking(x =>
                x.DeleteAsync(timer.UserId, timer.Name)
            ).Should()
            .ThrowAsync<NotFoundException>();
    }

    #endregion

    #region CalculateElapsedTime

    #region TestCases

    public static IEnumerable<TestCaseData> GetCalculateElapsedTimeCorrectTestCases()
    {
        yield return new TestCaseData(
            new TimerSessionDto[]
            {
                SessionFactory.CreateFromTimes(DateTime.UtcNow - TimeSpan.FromHours(5), DateTime.UtcNow - TimeSpan.FromHours(1)),
            }, TimeSpan.FromHours(4));

        yield return new TestCaseData(
            new TimerSessionDto[]
            {
                SessionFactory.CreateFromTimes(DateTime.UtcNow - TimeSpan.FromHours(18), DateTime.UtcNow - TimeSpan.FromHours(15)),
                SessionFactory.CreateFromTimes(DateTime.UtcNow - TimeSpan.FromHours(5), DateTime.UtcNow - TimeSpan.FromHours(1))
            }, TimeSpan.FromHours(7));

        yield return new TestCaseData(
            new TimerSessionDto[]
            {
                SessionFactory.CreateFromTimes(DateTime.UtcNow - TimeSpan.FromHours(8), DateTime.UtcNow - TimeSpan.FromHours(5)),
                SessionFactory.CreateFromTimes(DateTime.UtcNow - TimeSpan.FromHours(2), null)
            }, TimeSpan.FromHours(5));
    }

    #endregion

    [Test]
    [TestCaseSource(nameof(GetCalculateElapsedTimeCorrectTestCases))]
    public void CalculateElapsedTimeSessionsCorrect(TimerSessionDto[] sessions, TimeSpan expectedElapsedTime)
    {
        var timer = TimerFactory
            .CreateEmptyTimer()
            .WithSessions(sessions);
        _dateTimeProvider.UtcNow
            .Returns(DateTime.UtcNow);
        var result = _timersService.CalculateElapsedTime(timer);
        result
            .Should()
            .BeCloseTo(expectedElapsedTime, TimeSpan.FromSeconds(1));
    }

    [Test]
    public void CalculateElapsedTimeWithEmptySessionsReturnsZero()
    {
        var timer = TimerFactory
            .CreateEmptyTimer()
            .WithSessions([]);

        var result = _timersService.CalculateElapsedTime(timer);
        result
            .Should()
            .Be(TimeSpan.Zero);
    }

    #endregion
}