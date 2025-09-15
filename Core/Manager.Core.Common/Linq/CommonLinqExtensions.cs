using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Core.Common.Linq;

public static class CommonLinqExtensions
{
    public static void InsertBefore<TSource>(this IList<TSource> source, Func<TSource, bool> predicate, TSource element)
    {
        var beforeItem = source.FirstOrDefault(predicate);
        if (beforeItem == null)
        {
            return;
        }

        var insertIndex = source.IndexOf(beforeItem);
        source.Insert(insertIndex, element);
    }
}