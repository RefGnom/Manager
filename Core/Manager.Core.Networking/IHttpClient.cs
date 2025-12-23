using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Manager.Core.Common.DependencyInjection.Attributes;

namespace Manager.Core.Networking;

[IgnoreAutoRegistration]
public interface IHttpClient
{
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken = default);
}