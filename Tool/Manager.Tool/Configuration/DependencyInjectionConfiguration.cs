using System;
using Manager.Core.AppConfiguration.DependencyInjection.AutoRegistration;
using Manager.Core.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Tool.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceProvider ConfigureServiceCollection()
    {
        return new ServiceCollection()
            .UseAutoRegistrationForCurrentAssembly()
            .UseAutoRegistrationForAssembly<IDateTimeProvider>()
            .BuildServiceProvider();
    }
}