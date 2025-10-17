using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Manager.Core.Common.Linq;

public static class CommonLinqExtensions
{
    public static void InsertBefore<TSource>(this IList<TSource> source, Func<TSource, bool> predicate, TSource element)
    {
        var beforeItem = source.FirstOrDefault(predicate);

        var insertIndex = beforeItem != null ? source.IndexOf(beforeItem) : 0;
        source.Insert(insertIndex, element);
    }

    public static async Task<TOutput[]> SelectAsync<TSource, TOutput>(
        this IEnumerable<TSource> collection,
        Func<TSource, Task<TOutput>> func
    )
    {
        var results = new List<TOutput>();
        foreach (var element in collection)
        {
            var field = await func(element).ConfigureAwait(false);
            results.Add(field);
        }

        return results.ToArray();
    }
}