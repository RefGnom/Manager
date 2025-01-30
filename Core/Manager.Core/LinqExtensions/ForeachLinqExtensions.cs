namespace Manager.Core.LinqExtensions;

public static class ForeachLinqExtensions
{
    public static void Foreach<T>(IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source)
        {
            action(item);
        }
    }
}