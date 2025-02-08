using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.DbLayer;

public static class DbSetExtensions
{
    public static void AddOrUpdate<TDbo>(this DbSet<TDbo> dbSet, TDbo dbo, Func<TDbo, Guid> idPicker)
        where TDbo : class
    {
        if (dbSet.Any(x => idPicker(x) == idPicker(dbo)))
        {
            dbSet.Update(dbo);
        }
        else
        {
            dbSet.Add(dbo);
        }
    }
}