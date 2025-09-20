using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Manager.Core.AppConfiguration.DataBase;

public interface IDataContext
{
    Task InsertAsync<TEntity>(TEntity entity) where TEntity : class;
    Task<TEntity?> FindAsync<TEntity, TKey>(TKey primaryKey) where TEntity : class;

    Task<TEntity[]> SelectAsync<TEntity, TKey>(Func<TEntity, TKey> getKey, params TKey[] primaryKeys)
        where TEntity : class;

    Task<TResult> ExecuteReadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, Task<TResult>> func)
        where TEntity : class;

    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;
    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;
    Task DeleteAsync<TEntity>(Action<TEntity> setPrimaryKey) where TEntity : class, new();
}

internal class DataContext(
    IDbContextWrapperFactory dbContextWrapperFactory
) : IDataContext
{
    public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> FindAsync<TEntity, TKey>(TKey primaryKey) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        return await dbContext.FindAsync<TEntity>(primaryKey);
    }

    public async Task<TEntity[]> SelectAsync<TEntity, TKey>(Func<TEntity, TKey> getKey, params TKey[] primaryKeys)
        where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        return await dbContext.Set<TEntity>().Where(x => primaryKeys.Contains(getKey(x))).ToArrayAsync();
    }

    public async Task<TResult> ExecuteReadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, Task<TResult>> func)
        where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        return await func(dbContext.Set<TEntity>());
    }

    public async Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        dbContext.Update(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity>(Action<TEntity> setPrimaryKey) where TEntity : class, new()
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        var entity = new TEntity();
        setPrimaryKey(entity);
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }
}