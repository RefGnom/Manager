using System.Net;

namespace ManagerService.Client.ServiceModels;

public class HttpResponse
{
    public required HttpStatusCode StatusCode { get; set; }
    public bool IsSuccessStatusCode => (int)StatusCode >= 200 && (int)StatusCode <= 299;
}