using AutoMapper;
using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.Layers.DbLayer.Dbos;
using ManagerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.RepositoryLayer;

public class TimerRepository(ManagerDbContext dbContext, IMapper mapper) : ITimerRepository
{
    private readonly ManagerDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateOrUpdateAsync(TimerDto timerDto)
    {
        var timerDbo = _mapper.Map<TimerDto, TimerDbo>(timerDto);
        _dbContext.Timers.Add(timerDbo);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<TimerDto?> FindAsync(Guid userId, string timerName)
    {
        var timerDbo = await _dbContext.Timers
            .Where(x => x.UserId == userId)
            .Where(x => x.Name == timerName)
            .FirstAsync();
        return null!; // todo: Подключить автомаппет и тут превращать dbo -> dto
    }

    public async Task<TimerDto[]> SelectByUserAsync(Guid userId)
    {
        var timerDbos = await _dbContext.Timers
            .Where(x => x.UserId == userId)
            .ToArrayAsync();
        return []; // todo: сконвертить в dto
    }
}