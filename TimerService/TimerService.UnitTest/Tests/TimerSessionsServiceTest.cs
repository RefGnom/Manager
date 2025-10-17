using System;
using System.Threading.Tasks;
using FluentAssertions;
using Manager.Core.UnitTestsCore;
using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.Server.Layers.ServiceLayer.Services;
using Manager.TimerService.Server.ServiceModels;
using Manager.TimerService.UnitTest.Factories;
using NSubstitute;
using NUnit.Framework;

namespace Manager.TimerService.UnitTest.Tests;

public class TimerSessionsServiceTest : UnitTestBase
{
    [SetUp]
    public void Setup()
    {
        repository = Substitute.For<ITimerSessionRepository>();
        service = new TimerSessionService(repository);
    }

    private static readonly TimerSessionDtoTestFactory sessionFactory = new();

    private ITimerSessionRepository repository = null!;
    private TimerSessionService service = null!;

    [Test]
    public async Task StartSessionCorrect()
    {
        var session = sessionFactory.CreateEmptySession();
        await service.StartAsync(session.TimerId, session.StartTime);
        await repository
            .Received()
            .CreateAsync(
                Arg
                    .Is<TimerSessionDto>(x =>
                        x.TimerId == session.TimerId &&
                        x.StartTime == session.StartTime &&
                        x.StopTime == null &&
                        x.IsOver == false
                    )
            );
    }

    [Test]
    public async Task SelectByTimerCorrect()
    {
        var timerId = Guid.NewGuid();
        var session = sessionFactory.CreateEmptySession();
        repository
            .SelectByTimerAsync(timerId)
            .Returns([session]);

        var result = await service.SelectByTimerAsync(timerId);
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Has.Length.EqualTo(1));
        Assert.That(result[0], Is.EqualTo(session));
    }

    [Test]
    public async Task StopSessionCorrect()
    {
        var timerId = Guid.NewGuid();
        var session = sessionFactory.CreateEmptySession();
        repository
            .SelectByTimerAsync(timerId)
            .Returns([session]);

        await service.StopTimerSessionAsync(timerId, DateTime.UtcNow);
        await repository
            .Received()
            .UpdateAsync(
                Arg.Is<TimerSessionDto>(x => x.IsOver)
            );
    }

    [Test]
    public async Task StopTimerWithoutActiveSessionThrowsException()
    {
        var timerId = Guid.NewGuid();
        var session = sessionFactory.CreateEmptySession();
        session.IsOver = true;
        repository
            .SelectByTimerAsync(timerId)
            .Returns([session]);

        await service.Invoking(x =>
                x.StopTimerSessionAsync(timerId, DateTime.UtcNow)
            ).Should()
            .ThrowAsync<InvalidOperationException>();
    }
}