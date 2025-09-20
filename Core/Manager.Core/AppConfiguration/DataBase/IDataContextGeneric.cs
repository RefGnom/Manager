using System;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Core.AppConfiguration.DataBase;

public interface IDataContext<TEntity> where TEntity : class
{
    Task InsertAsync(TEntity entity);
    Task<TEntity?> FindAsync<TKey>(TKey primaryKey);
    Task<TEntity[]> SelectAsync<TKey>(Func<TEntity, TKey> getKey, params TKey[] primaryKeys);
    Task<TResult> ExecuteReadAsync<TResult>(Func<IQueryable<TEntity>, Task<TResult>> func);
    Task UpdateAsync(TEntity entity);

    /// <summary>
    /// Можно передавать сущность, где определён только первичный ключ, а все остальные значения default
    /// </summary>
    Task DeleteAsync(TEntity entity);

    /// <summary>
    /// Метод удаления по первичному ключу в дженерик дата контексте делает два запроса к БД.
    /// Если есть возможность, то используй метод удаления принимающий сущность либо дата контекст без дженерика
    /// </summary>
    Task DeleteAsync<TKey>(TKey primaryKey);
}

internal class DataContext<TEntity>(
    IDataContext dataContext,
    IDbContextWrapperFactory dbContextWrapperFactory
) : IDataContext<TEntity> where TEntity : class
{
    public Task InsertAsync(TEntity entity)
    {
        return dataContext.InsertAsync(entity);
    }

    public Task<TEntity?> FindAsync<TKey>(TKey primaryKey)
    {
        return dataContext.FindAsync<TEntity, TKey>(primaryKey);
    }

    public Task<TEntity[]> SelectAsync<TKey>(Func<TEntity, TKey> getKey, params TKey[] primaryKeys)
    {
        return dataContext.SelectAsync(getKey, primaryKeys);
    }

    public Task<TResult> ExecuteReadAsync<TResult>(Func<IQueryable<TEntity>, Task<TResult>> func)
    {
        return dataContext.ExecuteReadAsync(func);
    }

    public Task UpdateAsync(TEntity entity)
    {
        return dataContext.UpdateAsync(entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
        return dataContext.DeleteAsync(entity);
    }

    public async Task DeleteAsync<TKey>(TKey primaryKey)
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        var entity = await dbContext.Set<TEntity>().FindAsync(primaryKey);
        if (entity == null)
        {
            return;
        }

        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}