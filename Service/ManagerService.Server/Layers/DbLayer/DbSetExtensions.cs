using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.DbLayer;

public static class DbSetExtensions
{
    public static void AddOrUpdate<TDbo>(this DbSet<TDbo> dbSet, TDbo dbo, Func<TDbo, bool> comparer)
        where TDbo : class
    {
        if (dbSet.Any(x => comparer(x)))
        {
            dbSet.Update(dbo);
        }
        else
        {
            dbSet.Add(dbo);
        }
    }
}