using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TimerRepository : ITimerRepository
{
    private readonly ManagerDbContext _dbContext;

    public TimerRepository(ManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveAsync() => await _dbContext
        .SaveChangesAsync();

    public async Task DeleteAsync(Guid id)
    {
        await _dbContext.Timers
            .Where(item => item.Id == id)
            .ExecuteDeleteAsync();
        await SaveAsync();
    }

    public async Task<List<TimerEntity>> GetAllForUserAsync(Guid userId) =>
        await _dbContext.Timers
            .Where(t => t.UserId == userId)
            .ToListAsync();

    public async Task CreateAsync(TimerEntity timer) =>
        await _dbContext.Timers
            .AddAsync(timer);

    public async Task<TimerEntity> FindAsync(Guid id) =>
        await _dbContext.Timers
            .FindAsync(id);

    public async Task PingTimeAsync(Guid id, DateTime pingTime)
    {
        await _dbContext.Timers
            .Where(entity => entity.Id == id)
            .ExecuteUpdateAsync(s =>
                s.SetProperty(entity => entity.PingTime, pingTime));

        await SaveAsync();
    }
}