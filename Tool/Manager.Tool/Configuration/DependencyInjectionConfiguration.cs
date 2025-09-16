using System;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Tool.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceProvider ConfigureServiceCollection()
    {
        return new ServiceCollection()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForCoreCommon()
            .BuildServiceProvider();
    }
}