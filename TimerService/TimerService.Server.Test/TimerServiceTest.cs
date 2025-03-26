using FluentAssertions;
using Manager.Core.DateTimeProvider;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;
using Manager.TimerService.Server.Layers.ServiceLayer.Factories;
using Manager.TimerService.Server.Layers.ServiceLayer.Services;
using Manager.TimerService.Server.ServiceModels;
using NUnit.Framework;
using NSubstitute;
using TimerService.Server.Test.Factories;

namespace TimerService.Server.Test;

[TestFixture]
public class TimerServicesTest()
{
    private static ITimerDtoTestFactory _timerFactory = new TimerDtoTestFactory();
    private static ITimerSessionDtoTestFactory _sessionFactory = new TimerSessionDtoTestFactory();

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

    #region StartTimer

    [Test]
    public async Task StartCreatesAndStartsSessionCorrect()
    {
        var timer = _timerFactory.CreateEmptyTimer();
        await _timersService.StartAsync(timer);

        await _timerRepository
            .Received(1)
            .CreateAsync(Arg.Is<TimerDto>(
                x => x.Status == TimerStatus.Started
            ));
        await _timerSessionService
            .Received(1)
            .StartAsync(timer.Id, timer.StartTime!.Value);
    }

    [Test]
    public async Task StartTimerWithInvalidStatusThrowsException()
    {
        var timer = _timerFactory.CreateEmptyTimer();
        timer.Status = TimerStatus.Started;
        _timerRepository
            .FindAsync(Arg.Any<Guid>(), Arg.Any<string>())
            .Returns(timer);

        await _timersService
            .Invoking(x => x.StartAsync(timer))
            .Should()
            .ThrowAsync<InvalidStatusException>();
    }

    [Test]
    public async Task StartStoppedTimerCorrect()
    {
        var timer = _timerFactory.CreateEmptyTimer();
        _timerRepository
            .FindAsync(Arg.Any<Guid>(), Arg.Any<string>())
            .Returns(timer);
        timer.Status = TimerStatus.Stopped;
        await _timersService.StartAsync(timer);

        await _timerRepository
            .Received(1)
            .UpdateAsync(Arg.Is<TimerDto>(
                x => x.Status == TimerStatus.Started)
            );
        await _timerSessionService
            .Received(1)
            .StartAsync(timer.Id, timer.StartTime!.Value);
    }

    #endregion

    #region TestCases

    public static IEnumerable<TestCaseData> GetCalculateElapsedTimeWithCompletedSessionsCorrectTestCases()
    {
        yield return new TestCaseData(
            new TimerSessionDto[]
            {
                _sessionFactory.CreateFromTimes(DateTime.Parse("2023-10-01T10:00:00"), DateTime.Parse("2023-10-01T11:00:00"))
            }, TimeSpan.FromHours(1));

        yield return new TestCaseData(
            new TimerSessionDto[]
            {
                _sessionFactory.CreateFromTimes(DateTime.Parse("2023-10-01T10:00:00"), DateTime.Parse("2023-10-01T11:00:00")),
                _sessionFactory.CreateFromTimes(DateTime.Parse("2023-10-01T12:00:00"), DateTime.Parse("2023-10-01T13:00:00"))
            }, TimeSpan.FromHours(2));
    }

    public static IEnumerable<TestCaseData> GetCalculateElapsedTimeWithUnCompletedSessionsCorrectTestCases()
    {
        yield return new TestCaseData(
            new TimerSessionDto[]
            {
                _sessionFactory.CreateFromTimes(DateTime.Parse("2023-10-01T10:00:00"), null)
            });
        yield return new TestCaseData(
            new TimerSessionDto[]
            {
                _sessionFactory.CreateFromTimes(DateTime.Parse("2023-10-01T10:00:00"), DateTime.Parse("2023-10-01T11:00:00")),
                _sessionFactory.CreateFromTimes(DateTime.Parse("2023-10-01T12:00:00"), DateTime.Parse("2023-10-01T13:00:00")),
                _sessionFactory.CreateFromTimes(DateTime.Parse("2023-10-01T14:00:00"), null)
            });
    }

    #endregion

    [Test]
    [TestCaseSource(nameof(GetCalculateElapsedTimeWithCompletedSessionsCorrectTestCases))]
    public void CalculateElapsedTimeWithCompletedSessionsCorrect(TimerSessionDto[] sessions, TimeSpan expectedElapsedTime)
    {
        var timer = _timerFactory.CreateFromSessions(sessions);
        var result = _timersService.CalculateElapsedTime(timer);
        result
            .Should()
            .Be(expectedElapsedTime);
    }

    [Test]
    [TestCaseSource(nameof(GetCalculateElapsedTimeWithUnCompletedSessionsCorrectTestCases))]
    public void CalculateElapsedTimeWithUnCompletedSessionsCorrect(TimerSessionDto[] sessions)
    {
        var timer = _timerFactory.CreateFromSessions(sessions);
        _timersService
            .Invoking(service => service.CalculateElapsedTime(timer))
            .Should()
            .Throw<ArgumentNullException>();
    }

    [Test]
    public void CalculateElapsedTimeWithEmptySessionsReturnsZero()
    {
        var timer = _timerFactory.CreateFromSessions([]);
        var result = _timersService.CalculateElapsedTime(timer);
        result
            .Should()
            .Be(TimeSpan.Zero);
    }
}