using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Query;

namespace Manager.Core.EFCore;

public interface IDataContext<TEntity> where TEntity : class
{
    Task InsertAsync(TEntity entity);
    Task<TEntity?> FindAsync<TKey>(TKey primaryKey);
    Task<TEntity[]> SelectAsync<TKey>(Func<TEntity, TKey> getKey, params TKey[] primaryKeys);
    Task<TResult> ExecuteReadAsync<TResult>(Func<IQueryable<TEntity>, Task<TResult>> func);
    Task UpdateAsync(TEntity entity);

    Task UpdatePropertiesAsync<TKey>(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperties,
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] keys
    );

    /// <summary>
    /// Можно передавать сущность, где определён только первичный ключ, а все остальные значения default
    /// </summary>
    Task DeleteAsync(TEntity entity);

    Task DeleteAsync<TKey>(Expression<Func<TEntity, TKey>> primaryKeyPicker, params TKey[] keys);
}

internal class DataContext<TEntity>(
    IDataContext dataContext
) : IDataContext<TEntity> where TEntity : class
{
    public Task InsertAsync(TEntity entity) => dataContext.InsertAsync(entity);

    public Task<TEntity?> FindAsync<TKey>(TKey primaryKey) => dataContext.FindAsync<TEntity, TKey>(primaryKey);

    public Task<TEntity[]> SelectAsync<TKey>(Func<TEntity, TKey> getKey, params TKey[] primaryKeys)
        => dataContext.SelectAsync(getKey, primaryKeys);

    public Task<TResult> ExecuteReadAsync<TResult>(Func<IQueryable<TEntity>, Task<TResult>> func)
        => dataContext.ExecuteReadAsync(func);

    public Task UpdateAsync(TEntity entity) => dataContext.UpdateAsync(entity);

    public Task UpdatePropertiesAsync<TKey>(
        Expression<Func<SetPropertyCalls<TEntity>, SetPropertyCalls<TEntity>>> setProperties,
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        params TKey[] keys
    ) => dataContext.UpdatePropertiesAsync(setProperties, primaryKeyPicker, keys);

    public Task DeleteAsync(TEntity entity) => dataContext.DeleteAsync(entity);

    public Task DeleteAsync<TKey>(Expression<Func<TEntity, TKey>> primaryKeyPicker, params TKey[] keys)
        => dataContext.DeleteAsync(primaryKeyPicker, keys);
}