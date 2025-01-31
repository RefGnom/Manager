using Manager.Core.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Tool.Configuration;

public static class DependencyInjectionConfiguration
{
    public static IServiceProvider ConfigureServiceCollection()
    {
        return new ServiceCollection()
            .UseAutoRegistrationForCurrentAssembly()
            .BuildServiceProvider();
    }
}