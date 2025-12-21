# Архитектура сетевого взаимодействия

## Обзор

Этот документ описывает архитектуру сетевого взаимодействия в проекте. Цель состоит в том, чтобы предоставить надежный и отказоустойчивый способ взаимодействия между службами.

## Компоненты

### IHttpClient

`IHttpClient` - это интерфейс, который абстрагирует `HttpClient` и предоставляет единый метод `SendAsync` для отправки HTTP-запросов. Эта абстракция позволяет нам внедрять различные реализации `HttpClient`, например, для тестирования или для добавления дополнительной функциональности, такой как повторные попытки и fallback.

### ResilientHttpClient

`ResilientHttpClient` - это реализация `IHttpClient`, которая использует библиотеку [Polly](https://github.com/App-vNext/Polly) для обеспечения отказоустойчивости. Он реализует следующие политики:

*   **Retry:** Повторяет неудачные запросы до 3 раз с экспоненциальной задержкой. Это помогает справиться с временными сбоями в сети.
*   **Fallback:** Если все повторные попытки не увенчались успехом, возвращается ответ об ошибке сервера по умолчанию. Это предотвращает сбой приложения из-за сбоя службы.

### Конфигурация

Клиенты API должны быть настроены на использование `ResilientHttpClient` через внедрение зависимостей. Это позволяет нам централизованно управлять политиками отказоустойчивости для всех служб.

## Использование

Чтобы использовать `ResilientHttpClient`, просто внедрите `IHttpClient` в свой класс и используйте его для отправки HTTP-запросов.

```csharp
public class MyService
{
    private readonly IHttpClient _httpClient;

    public MyService(IHttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task DoSomethingAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, "https://example.com");
        var response = await _httpClient.SendAsync(request, CancellationToken.None);
        // ...
    }
}
```
