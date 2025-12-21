using Microsoft.AspNetCore.Builder;

namespace Manager.Core;

public static class CachingMiddlewareExtensions
{
    public static IApplicationBuilder UseCachingMiddleware(this IApplicationBuilder app) =>
        app.UseMiddleware<CachingMiddleware>();
}