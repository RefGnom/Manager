namespace Manager.Core.Caching;

public class RedisOptions
{
    public string? Host { get; init; }
    public int Port { get; init; } = 6379;
    public string? Password { get; init; }
    public string? TimeoutInMs { get; init; }
}