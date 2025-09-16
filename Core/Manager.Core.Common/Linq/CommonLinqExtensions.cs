using System;
using System.Collections.Generic;
using System.Linq;

namespace Manager.Core.Common.Linq;

public static class CommonLinqExtensions
{
    public static void InsertBefore<TSource>(this IList<TSource> source, Func<TSource, bool> predicate, TSource element)
    {
        var beforeItem = source.FirstOrDefault(predicate);

        var insertIndex = beforeItem != null ? source.IndexOf(beforeItem) : 0;
        source.Insert(insertIndex, element);
    }
}