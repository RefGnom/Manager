using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace Manager.Core.Networking;

public class HttpResult(
    HttpResponseMessage httpResponseMessage
)
{
    public HttpStatusCode StatusCode => httpResponseMessage.StatusCode;
    public bool IsSuccess => httpResponseMessage.IsSuccessStatusCode;
    public string ResultMessage => httpResponseMessage.ReasonPhrase ?? string.Empty;

    public static implicit operator HttpResult(HttpResponseMessage httpResponse) => new(httpResponse);
}

public class HttpResult<TResponse>(
    HttpResponseMessage httpResponseMessage,
    TResponse? response
) : HttpResult(httpResponseMessage)
{
    private readonly HttpResponseMessage httpResponseMessage = httpResponseMessage;

    public bool IsNotFound => httpResponseMessage.StatusCode == HttpStatusCode.NotFound;

    public TResponse EnsureResponse
    {
        get
        {
            httpResponseMessage.EnsureSuccessStatusCode();
            return response ?? throw new HttpRequestException();
        }
    }

    public static implicit operator HttpResult<TResponse>(HttpResponseMessage httpResponse) => new(
        httpResponse,
        httpResponse.IsSuccessStatusCode
            ? httpResponse.Content.ReadFromJsonAsync<TResponse>().GetAwaiter().GetResult()
            : default
    );
}