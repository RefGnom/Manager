using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.Layers.DbLayer.Dbos;
using ManagerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.RepositoryLayer;

public class TimerRepository(ManagerDbContext dbContext) : ITimerRepository
{
    private readonly ManagerDbContext _dbContext = dbContext;

    public async Task CreateAsync(TimerDbo dto)
    {
        await _dbContext.Timers.AddAsync(dto);
        await _dbContext.SaveChangesAsync();
    }

    public async Task CreateOrUpdateAsync(TimerDto timerDto)
    {
        var timerDbo = (TimerDbo)null!; // todo: Подключить автомаппер и тут превращать dto -> dbo
        _dbContext.Timers.AddOrUpdate(timerDbo, x => x.Id == timerDbo.Id);
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