using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.Common.DependencyInjection.LifetimeAttributes;
using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Services;

[Scoped]
public class TimerSessionService(
    ITimerSessionRepository repository
) : ITimerSessionService
{
    public async Task StartAsync(Guid timerId, DateTime startTime)
    {
        await repository.CreateAsync(
            new TimerSessionDto
            {
                Id = Guid.NewGuid(),
                TimerId = timerId,
                StartTime = startTime,
                StopTime = null,
                IsOver = false,
            }
        );
    }

    public async Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId)
    {
        return await repository.SelectByTimerAsync(timerId);
    }

    public async Task StopTimerSessionAsync(Guid timerId, DateTime stopTimer)
    {
        var timerSessions = await SelectByTimerAsync(timerId);
        var lastSession = timerSessions
            .Where(x => !x.IsOver)
            !.FirstOrDefault();
        if (lastSession is null)
        {
            throw new InvalidOperationException("Timer hasn't active sessions");
        }

        lastSession.StopTime = stopTimer;
        lastSession.IsOver = true;
        await repository.UpdateAsync(lastSession);
    }
}