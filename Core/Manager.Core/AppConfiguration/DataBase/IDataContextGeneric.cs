using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

    Task DeleteAsync<TKey>(Func<TEntity, TKey> primaryKeyPicker, params TKey[] keys);
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

    public async Task DeleteAsync<TKey>(Func<TEntity, TKey> primaryKeyPicker, params TKey[] keys)
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        await dbContext.Set<TEntity>().Where(x => keys.Contains(primaryKeyPicker(x))).ExecuteDeleteAsync();
        await dbContext.SaveChangesAsync();
    }
}