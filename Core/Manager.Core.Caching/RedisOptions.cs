namespace Manager.Core.Caching;

public class RedisOptions
{
    public string? Host { get; init; }
    public string? Password { get; init; }
    public string? TimeoutInMs { get; init; }
}