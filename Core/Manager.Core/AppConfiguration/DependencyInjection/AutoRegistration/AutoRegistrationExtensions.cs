using System.Linq;
using System.Reflection;
using Manager.Core.AppConfiguration.DependencyInjection.LifetimeAttributes;
using Manager.Core.Common.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.AppConfiguration.DependencyInjection.AutoRegistration;

public static class AutoRegistrationExtensions
{
    private const ServiceLifetime LifestyleByDefault = ServiceLifetime.Singleton;

    public static IServiceCollection UseAutoRegistrationForCurrentAssembly(this IServiceCollection serviceCollection)
    {
        return serviceCollection.UseAutoRegistrationForAssembly(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection UseAutoRegistrationForAssembly<TTypeFromAssembly>(
        this IServiceCollection serviceCollection
    )
    {
        var type = typeof(TTypeFromAssembly);
        var assembly = Assembly.GetAssembly(type);
        return assembly is null
            ? throw new AutoRegistrationException($"Не смогли определить сборку по типу {type}")
            : serviceCollection.UseAutoRegistrationForAssembly(assembly);
    }

    public static IServiceCollection UseAutoRegistrationForCoreCommon(
        this IServiceCollection serviceCollection
    )
    {
        var assembly = Assembly.Load("Manager.Core.Common");
        return serviceCollection.UseAutoRegistrationForAssembly(assembly);
    }

    private static IServiceCollection UseAutoRegistrationForAssembly(
        this IServiceCollection serviceCollection,
        Assembly assembly,
        string? namespacePrefix = null
    )
    {
        var serviceAssemblies = AssemblyProvider.GetServiceAssemblies();

        assembly.GetExportedTypes()
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .SelectMany(x => x.GetInterfaces()
                .Where(i => serviceAssemblies.Contains(i.Assembly))
                .Where(i => namespacePrefix is null || i.Namespace!.StartsWith(namespacePrefix))
                .Select(i =>
                    {
                        var lifetimeAttribute = x.GetCustomAttributes<LifetimeAttribute>().FirstOrDefault();
                        var lifetime = lifetimeAttribute?.Lifetime ?? LifestyleByDefault;
                        return new ServiceDescriptor(i, x, lifetime);
                    }
                )
            )
            .Select(x => x.TryConvertToGenericDefinition(out var descriptorWithGenericDefinition)
                ? descriptorWithGenericDefinition!
                : x
            )
            .Foreach(serviceCollection.Add);

        return serviceCollection;
    }

    private static bool TryConvertToGenericDefinition(
        this ServiceDescriptor descriptor,
        out ServiceDescriptor? descriptorWithGenericDefinition
    )
    {
        if (descriptor.ServiceType.IsGenericType && descriptor.ImplementationType!.IsGenericType)
        {
            descriptorWithGenericDefinition = new ServiceDescriptor(
                descriptor.ServiceType.GetGenericTypeDefinition(),
                descriptor.ImplementationType.GetGenericTypeDefinition(),
                descriptor.Lifetime
            );
            return true;
        }

        descriptorWithGenericDefinition = null;
        return false;
    }
}