using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.DependencyInjection.LifetimeAttributes;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Services;

[Scoped]
public class TimerSessionService(
    ITimerSessionRepository repository
)
    : ITimerSessionService
{
    private readonly ITimerSessionRepository _repository = repository;

    public async Task StartSessionAsync(Guid timerId, DateTime startTime)
    {
        await _repository.CreateAsync(
            new TimerSessionDto()
            {
                Id = Guid.NewGuid(),
                TimerId = timerId,
                StartTime = startTime,
                StopTime = null,
                IsOver = false
            }
        );
    }

    public async Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId)
    {
        return await _repository.SelectByTimerAsync(timerId);
    }

    public async Task StopTimerSessionAsync(Guid timerId, DateTime stopTimer)
    {
        var timerSessions = await SelectByTimerAsync(timerId);
        var lastSession = timerSessions
            .Where(x => x.IsOver == false)
            !.FirstOrDefault();
        if (lastSession is null || lastSession.IsOver)
        {
            throw new InvalidOperationException("Timer hasn't active sessions");
        }

        lastSession.StopTime = stopTimer;
        lastSession.IsOver = true;
        await _repository.UpdateAsync(lastSession);
    }
}