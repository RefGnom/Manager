using System.Collections.Generic;

namespace Manager.Core.Common.Linq;

public static class ToStringLinqExtensions
{
    public static string JoinToString<T>(this IEnumerable<T> source, char separator = ';') =>
        string.Join(separator, source);

    public static string JoinToString<T>(this IEnumerable<T> source, string separator = ", ") =>
        string.Join(separator, source);
}