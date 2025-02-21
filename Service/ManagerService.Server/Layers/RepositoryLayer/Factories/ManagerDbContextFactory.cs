using ManagerService.Server.Layers.DbLayer;
using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.RepositoryLayer.Factories;

public class ManagerDbContextFactory(DbContextOptions<ManagerDbContext> options)
    : IDbContextFactory<ManagerDbContext>
{
    private readonly DbContextOptions<ManagerDbContext> _options = options;

    public ManagerDbContext CreateDbContext()
    {
        return new ManagerDbContext(_options);
    }
}