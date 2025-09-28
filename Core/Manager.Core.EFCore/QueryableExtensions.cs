using System;
using System.Linq;
using System.Linq.Expressions;

namespace Manager.Core.EFCore;

public static class QueryableExtensions
{
    public static IQueryable<TEntity> WhereContains<TEntity, TKey>(
        this IQueryable<TEntity> queryable,
        Expression<Func<TEntity, TKey>> primaryKeyPicker,
        TKey[] keys
    )
    {
        return queryable.Where(WhereContainsExpressionCache<TEntity, TKey>.BuildWhereExpression(primaryKeyPicker, keys));
    }
}