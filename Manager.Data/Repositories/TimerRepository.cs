using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TimerRepository : IRepository<TimerEntity>
{
    private readonly ManagerDbContext _dbContext;

    public TimerRepository(ManagerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TimerEntity>> GetAllAsync() => await _dbContext.Timers
        .ToListAsync();
    public async Task<TimerEntity> TryGetByIdAsync(Guid id) => await _dbContext.Timers
        .FindAsync(id);

    public async Task CreateAsync(TimerEntity item)
    {
        await _dbContext.Timers
            .AddAsync(item);
        await SaveAsync();
    }

    public async Task UpdateAsync(Guid id, TimerEntity updateItem)
    {
        await _dbContext.Timers
            .Where(entity => entity.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(entity => entity.Name, updateItem.Name)
                .SetProperty(entity => entity.StartTime, updateItem.StartTime)
                .SetProperty(entity => entity.PingTime, updateItem.PingTime)
            );
        await SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _dbContext.Timers
            .Where(entity => entity.Id == id)
            .ExecuteDeleteAsync();
        await SaveAsync();
    }

    public async Task SaveAsync() => await _dbContext
        .SaveChangesAsync();
}