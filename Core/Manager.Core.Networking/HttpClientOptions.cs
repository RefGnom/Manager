namespace Manager.Core.Networking;

public class HttpClientOptions
{
    /// <summary>
    /// Единый флаг. Если true:
    /// 1. Включает FallbackMiddleware (сервер отдает 503 при падении).
    /// 2. Включает FallbackPolicy в HttpClient (клиент возвращает 503 при недоступности сети).
    /// </summary>
    public bool EnableFallback { get; set; } = false;

    public int RetryCount { get; set; } = 3;

    public int? RetryDelayMs { get; set; }

    public int? TimeoutMs { get; set; }
}