using System;
using System.Collections.Generic;

namespace Manager.Core.LinqExtensions;

public static class ForeachLinqExtensions
{
    public static void Foreach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }
}