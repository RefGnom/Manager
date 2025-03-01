using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Manager.Core.Extensions.LinqExtensions;

public static class ForeachLinqExtensions
{
    public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }

    public static async Task ForeachAsync<T>(this IEnumerable<T> source, Func<T, Task> action)
    {
        foreach (var item in source)
        {
            await action(item);
        }
    }
}