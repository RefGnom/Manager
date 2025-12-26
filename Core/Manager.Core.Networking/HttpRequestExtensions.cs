using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Manager.Core.Networking;

public static class HttpRequestExtensions
{
    public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage httpRequestMessage)
    {
        var clone = new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri);

        if (httpRequestMessage.Content != null)
        {
            var ms = new System.IO.MemoryStream();
            await httpRequestMessage.Content.CopyToAsync(ms);
            ms.Position = 0;
            clone.Content = new StreamContent(ms);
            foreach (var h in httpRequestMessage.Content.Headers)
            {
                clone.Content.Headers.Add(h.Key, h.Value);
            }
        }

        clone.Version = httpRequestMessage.Version;
        foreach (var prop in httpRequestMessage.Options)
        {
            clone.Options.TryAdd(prop.Key, prop.Value);
        }

        foreach (var header in httpRequestMessage.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        return clone;
    }
}