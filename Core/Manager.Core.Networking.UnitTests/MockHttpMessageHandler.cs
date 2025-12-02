using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Manager.Core.Networking.UnitTests;

public class MockHttpMessageHandler(
    Func<HttpRequestMessage, CancellationToken, Task<HttpResponseMessage>> handler
)
    : HttpMessageHandler
{
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        return handler(request, cancellationToken);
    }
}
