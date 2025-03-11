using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manager.Core.DependencyInjection.LifetimeAttributes;
using Manager.TimerService.Server.Layers.DbLayer;
using Manager.TimerService.Server.Layers.DbLayer.Dbos;
using Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;
using Manager.TimerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Manager.TimerService.Server.Layers.RepositoryLayer;

[Scoped]
public class TimerRepository(
    ManagerDbContext dbContext,
    IMapper mapper
) : ITimerRepository
{
    private readonly ManagerDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task<TimerDto[]> SelectByUserAsync(Guid userId)
    {
        return await _dbContext.Timers
            .Where(x => x.UserId == userId)
            .Select(x => _mapper.Map<TimerDto>(x))
            .ToArrayAsync();
    }

    public async Task CreateAsync(TimerDto timerDto)
    {
        var timerDbo = _mapper.Map<TimerDto, TimerDbo>(timerDto);
        _dbContext.Timers.Add(timerDbo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimerDto timerDto)
    {
        var existedTimer = await FindAsync(timerDto.Id);
        if (existedTimer is null)
        {
            throw new NotFoundException("Timer not found");
        }

        await _dbContext.Timers
            .Where(x => x.Id == existedTimer.Id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(entity => entity.Name, timerDto.Name)
                    .SetProperty(entity => entity.StartTime, timerDto.StartTime)
                    .SetProperty(entity => entity.PingTimeout, timerDto.PingTimeout)
                    .SetProperty(entity => entity.Status, timerDto.Status)
            );
    }

    public async Task<TimerDto?> FindAsync(Guid userId, string timerName)
    {
        var timerDbo = await _dbContext.Timers
            .Where(x => x.UserId == userId)
            .Where(x => x.Name == timerName)
            .FirstOrDefaultAsync();
        return _mapper.Map<TimerDto>(timerDbo);
    }

    private async Task<TimerDbo> FindAsync(Guid id)
    {
        return _mapper.Map<TimerDbo>(
            await _dbContext.Timers.FindAsync(id)
        );
    }
}