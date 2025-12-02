using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Core.Networking;

public interface IHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}