using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manager.Core.Common.DependencyInjection.LifetimeAttributes;
using Manager.TimerService.Server.Layers.DbLayer;
using Manager.TimerService.Server.Layers.DbLayer.Dbos;
using Manager.TimerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Manager.TimerService.Server.Layers.RepositoryLayer;

[Scoped]
public class TimerSessionRepository(
    IMapper mapper,
    ManagerDbContext dbContext
) : ITimerSessionRepository
{
    private readonly ManagerDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateAsync(TimerSessionDto timerSessionDto)
    {
        var sessionDbo = _mapper.Map<TimerSessionDbo>(timerSessionDto);
        _dbContext.TimerSessions.Add(sessionDbo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimerSessionDto timerSessionDto)
    {
        var sessionDbo = _mapper.Map<TimerSessionDbo>(timerSessionDto);
        await _dbContext.TimerSessions
            .ExecuteUpdateAsync(
                s =>
                    s.SetProperty(entity => entity.StopTime, sessionDbo.StopTime)
                        .SetProperty(entity => entity.IsOver, sessionDbo.IsOver)
            );
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TimerSessionDto[]> SelectByTimerAsync(Guid timerId)
    {
        return await _dbContext.TimerSessions
            .Where(x => x.TimerId == timerId)
            .Select(x => _mapper.Map<TimerSessionDto>(x))
            .ToArrayAsync();
    }
}