namespace Manager.Core.LinqExtensions;

public static class ToStringLinqExtensions
{
    public static string JoinToString<T>(IEnumerable<T> source, char separator = ';') => string.Join(separator, source);
    public static string JoinToString<T>(IEnumerable<T> source, string separator = ", ") => string.Join(separator, source);
}