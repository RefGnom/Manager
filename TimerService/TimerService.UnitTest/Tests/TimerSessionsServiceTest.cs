using System;
using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.UnitTest.Factories;
using System.Threading.Tasks;
using Manager.TimerService.Server.Layers.ServiceLayer.Services;
using Manager.TimerService.Server.ServiceModels;
using NSubstitute;
using NUnit.Framework;
using FluentAssertions;

namespace Manager.TimerService.UnitTest.Tests;

[TestFixture]
public class TimerSessionsServiceTest
{
    private static readonly ITimerSessionDtoTestFactory SessionFactory = new TimerSessionDtoTestFactory();

    private ITimerSessionRepository _repository;
    private TimerSessionService _service;

    [SetUp]
    public void Setup()
    {
        _repository = Substitute.For<ITimerSessionRepository>();
        _service = new TimerSessionService(_repository);
    }

    #region Start

    [Test]
    public async Task StartSessionCorrect()
    {
        var session = SessionFactory.CreateEmptySession();
        await _service.StartAsync(session.TimerId, session.StartTime);
        await _repository
            .Received()
            .CreateAsync(Arg
                .Is<TimerSessionDto>(x =>
                    x.TimerId == session.TimerId &&
                    x.StartTime == session.StartTime &&
                    x.StopTime == null &&
                    x.IsOver == false
                )
            );
    }

    #endregion

    #region SelectByTimer

    [Test]
    public async Task SelectByTimerCorrect()
    {
        var timerId = Guid.NewGuid();
        var session = SessionFactory.CreateEmptySession();
        _repository
            .SelectByTimerAsync(timerId)
            .Returns(new[] { session });

        var result = await _service.SelectByTimerAsync(timerId);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Length, Is.EqualTo(1));
        Assert.That(result[0], Is.EqualTo(session));
    }

    #endregion

    #region Stop

    [Test]
    public async Task StopSessionCorrect()
    {
        var timerId = Guid.NewGuid();
        var session = SessionFactory.CreateEmptySession();
        _repository
            .SelectByTimerAsync(timerId)
            .Returns(new[] { session });

        await _service.StopTimerSessionAsync(timerId, DateTime.UtcNow);
        await _repository
            .Received()
            .UpdateAsync(
                Arg.Is<TimerSessionDto>(x => x.IsOver)
            );
    }

    [Test]
    public async Task StopTimerWithoutActiveSessionThrowsException()
    {
        var timerId = Guid.NewGuid();
        var session = SessionFactory.CreateEmptySession();
        session.IsOver = true;
        _repository
            .SelectByTimerAsync(timerId)
            .Returns(new[] { session });

        await _service.Invoking(x =>
                x.StopTimerSessionAsync(timerId, DateTime.UtcNow)
            ).Should()
            .ThrowAsync<InvalidOperationException>();
    }

    #endregion
}