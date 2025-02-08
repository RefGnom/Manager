using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.DbLayer;

public static class DbSetExtensions
{
    public static void AddOrUpdate<TDbo>(this DbSet<TDbo> dbSet, TDbo dbo, Func<TDbo, object> primaryKeyPicker)
        where TDbo : class
    {
        var primaryKey = primaryKeyPicker(dbo);
        if (dbSet.Find(primaryKey) is not null)
        {
            dbSet.Update(dbo);
        }
        else
        {
            dbSet.Add(dbo);
        }
    }
}