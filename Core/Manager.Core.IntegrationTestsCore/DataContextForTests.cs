using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Manager.Core.EFCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Manager.Core.IntegrationTestsCore;

public class DataContextForTests(
    DataContext innerContext
) : IDataContext
{
    public readonly List<object> Entities = [];

    public async Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
    {
        await innerContext.InsertAsync(entity);
        Entities.Add(entity);
    }

    public async Task InsertRangeAsync<TEntity>(params TEntity[] entities) where TEntity : class
    {
        await innerContext.InsertRangeAsync(entities);
        Entities.AddRange(entities);
    }

    public Task<TEntity?> FindAsync<TEntity, TKey>(TKey primaryKey) where TEntity : class
        => innerContext.FindAsync<TEntity, TKey>(primaryKey);

    public Task<TEntity[]> SelectAsync<TEntity, TKey>(
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] primaryKeys
    ) where TEntity : class
        => innerContext.SelectAsync(primaryKeyPicker, primaryKeys);

    public Task<TResult> ExecuteReadAsync<TEntity, TResult>(Func<IQueryable<TEntity>, Task<TResult>> func)
        where TEntity : class
        => innerContext.ExecuteReadAsync(func);

    public Task UpdateAsync<TEntity>(TEntity entity) where TEntity : class => innerContext.UpdateAsync(entity);

    public Task UpdatePropertiesAsync<TEntity, TKey>(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperties,
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] keys
    ) where TEntity : class
        => innerContext.UpdatePropertiesAsync(setProperties, primaryKeyPicker, keys);

    public Task DeleteAsync<TEntity>(TEntity entity) where TEntity : class => innerContext.DeleteAsync(entity);

    public Task DeleteAsync<TEntity, TKey>(Expression<Func<TEntity, TKey>> primaryKeyPicker, params TKey[] keys)
        where TEntity : class
        => innerContext.DeleteAsync(primaryKeyPicker, keys);
}