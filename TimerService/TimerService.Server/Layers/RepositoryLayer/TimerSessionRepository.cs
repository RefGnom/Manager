using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manager.Core.Common.DependencyInjection.Attributes;
using Manager.Core.EFCore;
using Manager.TimerService.Server.Layers.DbLayer.Dbos;
using Manager.TimerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Manager.TimerService.Server.Layers.RepositoryLayer;

[Scoped]
public class TimerSessionRepository(
    IMapper mapper,
    IDbContextWrapperFactory dbContextWrapperFactory
) : ITimerSessionRepository
{
    public async Task CreateAsync(TimerSessionDto timerSessionDto)
    {
        var dbContext = dbContextWrapperFactory.Create();
        var sessionDbo = mapper.Map<TimerSessionDbo>(timerSessionDto);
        dbContext.Set<TimerSessionDbo>().Add(sessionDbo);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimerSessionDto timerSessionDto)
    {
        var dbContext = dbContextWrapperFactory.Create();
        var sessionDbo = mapper.Map<TimerSessionDbo>(timerSessionDto);
        await dbContext.Set<TimerSessionDbo>()
            .ExecuteUpdateAsync(s =>
                s.SetProperty(entity => entity.StopTime, sessionDbo.StopTime)
                    .SetProperty(entity => entity.IsOver, sessionDbo.IsOver)
            );
        await dbContext.SaveChangesAsync();
    }

    public async Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId)
    {
        var dbContext = dbContextWrapperFactory.Create();
        return await dbContext.Set<TimerSessionDbo>()
            .Where(x => x.TimerId == timerId)
            .Select(x => mapper.Map<TimerSessionDto>(x))
            .ToArrayAsync();
    }
}