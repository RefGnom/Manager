using Microsoft.EntityFrameworkCore;

namespace Data;

public class ManagerDbContext : DbContext
{
    public ManagerDbContext(DbContextOptions<ManagerDbContext> options) : base(options)
    {
    }

    public DbSet<TimerEntity> Timers { get; set; }
    public DbSet<TimerSessionEntity> TimerSessions { get; set; }
}