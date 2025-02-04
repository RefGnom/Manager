using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TimerSessionRepository : IRepository<TimerSessionEntity>
{
    private readonly ManagerDbContext _dbContext;

    public TimerSessionRepository(ManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TimerSessionEntity>> GetAllAsync() => await _dbContext.TimerSessions
        .ToListAsync();


    public async Task<TimerSessionEntity> TryGetByIdAsync(Guid id) => await _dbContext.TimerSessions
        .FindAsync(id);

    public async Task CreateAsync(TimerSessionEntity item)
    {
        await _dbContext.TimerSessions
            .AddAsync(item);
        await SaveAsync();
    }

    public async Task UpdateAsync(Guid id, TimerSessionEntity item)
    {
        await _dbContext.TimerSessions
            .Where(entity => entity.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(entity => entity.StopTime, item.StopTime)
                .SetProperty(entity => entity.Ticks, item.Ticks)
            );
        await SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dbContext.TimerSessions
            .Where(item => item.Id == id)
            .ExecuteDeleteAsync();
        await SaveAsync();
    }

    public async Task SaveAsync() => await _dbContext
        .SaveChangesAsync();
}