using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manager.Core.DependencyInjection.LifetimeAttributes;
using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.Layers.DbLayer.Dbos;
using ManagerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.RepositoryLayer;

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
        _dbContext.Update(sessionDbo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TimerDto?> LastOrDefaultForTimerAsync(Guid timerId)
    {
        var sessionDbo = await _dbContext.TimerSessions
            .Where(x => x.TimerId == timerId)
            .OrderBy(s => s.StartTime)
            .LastOrDefaultAsync();
        return _mapper.Map<TimerDto>(sessionDbo);
    }

    public async Task<TimerSessionDto[]> SelectByTimer(Guid timerId)
    {
        return await _dbContext.TimerSessions
            .Where(x => x.TimerId == timerId)
            .Select(x => _mapper.Map<TimerSessionDto>(x))
            .ToArrayAsync();
    }
}