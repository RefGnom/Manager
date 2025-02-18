using ManagerService.Server.Layers.DbLayer.Dbos;
using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.DbLayer;

public class ManagerDbContext(DbContextOptions<ManagerDbContext> options) : DbContext(options)
{
    public DbSet<TimerDbo> Timers { get; set; }
    public DbSet<TimerSessionDbo> TimerSessions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TimerDbo>()
            .HasKey(t => new { t.UserId, t.Name });
    }
}