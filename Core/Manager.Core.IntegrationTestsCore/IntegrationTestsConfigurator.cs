using System;
using System.IO;
using System.Reflection;
using Manager.Core.Common.DependencyInjection.AutoRegistration;
using Manager.Core.EFCore;
using Manager.Core.EFCore.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Manager.Core.IntegrationTestsCore;

public static class IntegrationTestsConfigurator
{
    public static IServiceCollection ConfigureForIntegrationTests(
        this IServiceCollection serviceCollection,
        IConfigurationManager configuration,
        Assembly targetTestingAssembly,
        bool useNullLogger
    )
    {
        return serviceCollection
            .AddSingleton<IConfiguration>(configuration)
            .AddLogging(configuration, useNullLogger)
            .UseAutoRegistrationForAssembly(targetTestingAssembly)
            .UseAutoRegistrationForAssembly(GetTestsAssembly())
            .UseAutoRegistrationForCoreCommon()
            .ConfigureDb()
            .AddSingleton<IDbContextConfigurator, NpgTestingDbContextConfigurator>(x
                => new NpgTestingDbContextConfigurator(
                    x.GetRequiredService<IOptions<DataBaseOptions>>(),
                    x.GetRequiredService<ILogger<DbContextConfiguratorBase>>()
                ) { EntitiesAssembly = targetTestingAssembly }
            );
    }

    private static Assembly GetTestsAssembly()
    {
        var testAssemblyName = AppContext.BaseDirectory.Split(Path.DirectorySeparatorChar)[^5];
        var testName = $"Manager.{testAssemblyName}";
        return Assembly.Load(testName);
    }
}