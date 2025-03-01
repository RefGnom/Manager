using System.Net;

namespace Manager.TimerService.Client.ServiceModels;

public class HttpResponse
{
    public required HttpStatusCode StatusCode { get; set; }
    public string? ResponseMessage { get; set; }
    public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;
}