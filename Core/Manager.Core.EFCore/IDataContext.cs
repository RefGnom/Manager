using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Manager.Core.EFCore;

/// <summary>
///     Singleton зависимость для работы с БД через EFCore. В каждом методе создаёт свой скоуп
/// </summary>
public interface IDataContext
{
    Task InsertAsync<TEntity>(TEntity entity) where TEntity : class;
    Task InsertRangeAsync<TEntity>(params TEntity[] entities) where TEntity : class;
    Task<TEntity?> FindAsync<TEntity, TKey>(TKey primaryKey) where TEntity : class;

    Task<TEntity[]> SelectAsync<TEntity, TKey>(
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] primaryKeys
    ) where TEntity : class;

    Task<TResult> ExecuteReadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, Task<TResult>> func)
        where TEntity : class;

    Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class;

    Task UpdatePropertiesAsync<TEntity, TKey>(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperties,
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] keys
    ) where TEntity : class;

    Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class;

    Task DeleteAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> primaryKeyPicker, params TKey[] keys)
        where TEntity : class;
}

public class DataContext(
    IDbContextWrapperFactory dbContextWrapperFactory
) : IDataContext
{
    public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        await dbContext.AddAsync(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task InsertRangeAsync<TEntity>(params TEntity[] entities) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        // ReSharper disable once CoVariantArrayConversion
        await dbContext.AddRangeAsync(entities);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TEntity?> FindAsync<TEntity, TKey>(TKey primaryKey) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        return await dbContext.FindAsync<TEntity>(primaryKey);
    }

    public async Task<TEntity[]> SelectAsync<TEntity, TKey>(
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] primaryKeys
    ) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        return await dbContext.Set<TEntity>().WhereContains(primaryKeyPicker, primaryKeys).ToArrayAsync();
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

    public async Task UpdatePropertiesAsync<TEntity, TKey>(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperties,
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] keys
    ) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();

        await dbContext.Set<TEntity>()
            .WhereContains(primaryKeyPicker, keys)
            .ExecuteUpdateAsync(setProperties);
    }

    public async Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();
        dbContext.Remove(entity);
        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> primaryKeyPicker, params TKey[] keys)
        where TEntity : class
    {
        await using var dbContext = dbContextWrapperFactory.Create();

        await dbContext.Set<TEntity>()
            .WhereContains(primaryKeyPicker, keys)
            .ExecuteDeleteAsync();
    }
}