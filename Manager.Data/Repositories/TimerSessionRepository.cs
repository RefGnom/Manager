using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TimerSessionRepository : ITimerSessionRepository
{
    private readonly ManagerDbContext _dbContext;

    public TimerSessionRepository(ManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(TimerSessionEntity item)
    {
        await _dbContext.TimerSessions
            .AddAsync(item);

        await SaveAsync();
    }

    public async Task<TimerSessionEntity> FindAsync(Guid id) =>
        await _dbContext.TimerSessions
            .FindAsync(id);

    public async Task DeleteAsync(Guid id)
    {
        await _dbContext.TimerSessions
            .Where(item => item.Id == id)
            .ExecuteDeleteAsync();

        await SaveAsync();
    }

    public async Task SaveAsync() =>
        await _dbContext
            .SaveChangesAsync();

    public async Task<List<TimerSessionEntity>> GetForTimersAsync(Guid timerId) =>
        await _dbContext.TimerSessions
            .Where(entity => entity.TimerId == timerId)
            .ToListAsync();

    public async Task StopSessionAsync(Guid id, DateTime stopTime)
    {
        await _dbContext.TimerSessions
            .Where(entity => entity.Id == id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(entity => entity.StopTime, stopTime));

        await SaveAsync();
    }
}