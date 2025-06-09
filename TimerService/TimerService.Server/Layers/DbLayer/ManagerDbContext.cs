using Manager.TimerService.Server.Layers.DbLayer.Dbos;
using Microsoft.EntityFrameworkCore;

namespace Manager.TimerService.Server.Layers.DbLayer;

public class ManagerDbContext(DbContextOptions<ManagerDbContext> options) : DbContext(options)
{
    public DbSet<TimerDbo> Timers { get; set; }
    public DbSet<TimerSessionDbo> TimerSessions { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TimerDbo>()
            .HasIndex(x => new { x.UserId, x.Name })
            .IsUnique();
    }
}