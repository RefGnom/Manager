using Manager.Core.DateTimeProvider;
using Manager.TimerService.Server.Layers.RepositoryLayer;
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
        Assert.That(result, Is.EqualTo(expectedElapsedTime));
    }

    [Test]
    [TestCaseSource(nameof(GetCalculateElapsedTimeWithUnCompletedSessionsCorrectTestCases))]
    public void CalculateElapsedTimeWithUnCompletedSessionsCorrect(TimerSessionDto[] sessions)
    {
        var timer = _timerFactory.CreateFromSessions(sessions);
        Assert.Throws<ArgumentNullException>(() => _timersService.CalculateElapsedTime(timer));
    }

    [Test]
    public void CalculateElapsedTimeWithEmptySessionsReturnsZero()
    {
        var timer = _timerFactory.CreateFromSessions(new TimerSessionDto[0]);
        Assert.That(_timersService.CalculateElapsedTime(timer), Is.EqualTo(TimeSpan.Zero));
    }
}