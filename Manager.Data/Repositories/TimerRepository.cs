using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class TimerRepository : IRepository<TimerEntity>
{
    private readonly ManagerDbContext _context;

    public TimerRepository(ManagerDbContext context)
    {
        _context = context;
    }

    public async Task<List<TimerEntity>> GetAllAsync() => await _context.Timers.ToListAsync();

    public async Task<TimerEntity> TryGetByIdAsync(Guid id)
    {
        return await _context.Timers.FindAsync(id);
    }

    public async Task CreateAsync(TimerEntity item)
    {
        await _context.Timers.AddAsync(item);
        await SaveAsync();
    }

    public async Task UpdateAsync(Guid id, TimerEntity updateItem)
    {
        await _context.Timers.Where(entity => entity.Id == id)
            .ExecuteUpdateAsync(s => s
                .SetProperty(entity => entity.Name, updateItem.Name)
                .SetProperty(entity => entity.StartTime, updateItem.StartTime)
                .SetProperty(entity => entity.PingTime, updateItem.PingTime)
            );
        await SaveAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _context.Timers
            .Where(entity => entity.Id == id)
            .ExecuteDeleteAsync();
        await SaveAsync();
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();
}