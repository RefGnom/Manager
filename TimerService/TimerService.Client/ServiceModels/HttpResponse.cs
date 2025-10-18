using System.Net;
using System.Net.Http;

namespace Manager.TimerService.Client.ServiceModels;

public class HttpResponse
{
    public required HttpStatusCode StatusCode { get; set; }
    public string? ResponseMessage { get; set; }
    public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;

    public static HttpResponse Create(HttpResponseMessage httpResponseMessage) => new()
    {
        StatusCode = httpResponseMessage.StatusCode,
        ResponseMessage = httpResponseMessage.ReasonPhrase,
    };

    public static HttpResponse CreateOk() => new()
    {
        StatusCode = HttpStatusCode.OK,
    };
}