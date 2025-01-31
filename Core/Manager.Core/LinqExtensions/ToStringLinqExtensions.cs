using System.Collections.Generic;

namespace Manager.Core.LinqExtensions;

public static class ToStringLinqExtensions
{
    public static string JoinToString<T>(this IEnumerable<T> source, char separator = ';')
    {
        return string.Join(separator, source);
    }

    public static string JoinToString<T>(this IEnumerable<T> source, string separator = ", ")
    {
        return string.Join(separator, source);
    }
}