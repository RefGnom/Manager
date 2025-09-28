using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Manager.Core.EFCore;

public static class WhereContainsExpressionCache<TEntity, TKey>
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly MethodInfo containsMethod;

    private static readonly Func<Expression<Func<TEntity, TKey>>, TKey[], Expression<Func<TEntity, bool>>>
        expressionFactory;

    static WhereContainsExpressionCache()
    {
        containsMethod = typeof(Enumerable).GetMethods()
            .First(m => m.Name == "Contains" && m.GetParameters().Length == 2)
            .MakeGenericMethod(typeof(TKey));

        expressionFactory = CompileExpressionFactory();
    }

    public static Expression<Func<TEntity, bool>> BuildWhereExpression(
        Expression<Func<TEntity, TKey>> primaryKeySelector,
        TKey[] keys
    )
    {
        return expressionFactory(primaryKeySelector, keys);
    }

    private static Func<Expression<Func<TEntity, TKey>>, TKey[], Expression<Func<TEntity, bool>>>
        CompileExpressionFactory()
    {
        var pkPickerParameter = Expression.Parameter(typeof(Expression<Func<TEntity, TKey>>), "pkPicker");
        var keysParameter = Expression.Parameter(typeof(TKey[]), "keys");

        var entityParameter = Expression.Parameter(typeof(TEntity), "entity");
        var pkPickerInvocation = Expression.Invoke(pkPickerParameter, entityParameter);
        var containsCall = Expression.Call(containsMethod, keysParameter, pkPickerInvocation);
        var lambda = Expression.Lambda<Func<TEntity, bool>>(containsCall, entityParameter);

        return Expression.Lambda<Func<Expression<Func<TEntity, TKey>>, TKey[], Expression<Func<TEntity, bool>>>>(
            lambda,
            pkPickerParameter,
            keysParameter
        ).Compile();
    }
}