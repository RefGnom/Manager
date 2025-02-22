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
public class TimerRepository(
    ManagerDbContext dbContext,
    IMapper mapper
) : ITimerRepository
{
    private readonly ManagerDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateOrUpdateAsync(TimerDto timerDto)
    {
        var timerDbo = _mapper.Map<TimerDto, TimerDbo>(timerDto);

        var existedTimer = await FindAsync(timerDto.UserId, timerDto.Name);
        if (existedTimer is null)
        {
            _dbContext.Timers.Add(timerDbo);
        }
        else
        {
            _dbContext.Timers.Update(timerDbo);
        }

        await _dbContext.SaveChangesAsync();
    }

    public Task<TimerDto[]> SelectByUserAsync(Guid userId)
    {
        return _dbContext.Timers
            .Where(x => x.UserId == userId)
            .Select(x => _mapper.Map<TimerDto>(x))
            .ToArrayAsync();
    }

    public async Task<TimerDto?> FindAsync(Guid userId, string timerName)
    {
        var timerDbo = await _dbContext.Timers
            .Where(x => x.UserId == userId)
            .Where(x => x.Name == timerName)
            .FirstOrDefaultAsync();
        return _mapper.Map<TimerDto>(timerDbo);
    }
}