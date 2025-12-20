using System;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Manager.Core.Telemetry;

public static class ServiceCollectionExtensions
{
    public static void AddTelemetry<TResourceFiller>(this IServiceCollection serviceCollection)
        where TResourceFiller : class, IResourcesFactory
    {
        serviceCollection
            .AddSingleton<IResourceDetector, ResourceDetector>()
            .AddSingleton<IResourcesFactory, TResourceFiller>()
            .AddOpenTelemetry()
            .ConfigureResource(builder => builder.AddDetector(s => s.GetRequiredService<IResourceDetector>()))
            .UseOtlpExporter(TelemetryOptions.Protocol, new Uri(TelemetryOptions.EndPoint))
            .WithTracing(tracerProviderBuilder => tracerProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
                .SetSampler(new AlwaysOnSampler())
                .SetErrorStatusOnException()
            ).WithMetrics(meterProviderBuilder => meterProviderBuilder
                .AddAspNetCoreInstrumentation()
                .AddHttpClientInstrumentation()
            );
    }
}