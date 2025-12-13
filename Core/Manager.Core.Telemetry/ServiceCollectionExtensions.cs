using System;
using Manager.Core.Common;
using Microsoft.Extensions.DependencyInjection;
using OpenTelemetry;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Manager.Core.Telemetry;

public static class ServiceCollectionExtensions
{
    public static void AddTelemetry(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddOpenTelemetry()
            .ConfigureResource(r => r.AddService(serviceName: ManagerApp.FriendlyName))
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