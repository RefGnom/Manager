using System.Linq;
using System.Reflection;
using Manager.Core.Extensions.LinqExtensions;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.DependencyInjection;

public static class AutoRegistrationExtensions
{
    private const ServiceLifetime LifestyleByDefault = ServiceLifetime.Singleton;

    public static IServiceCollection UseAutoRegistrationForCurrentAssembly(this IServiceCollection serviceCollection)
    {
        return serviceCollection.UseAutoRegistrationForAssembly(Assembly.GetCallingAssembly());
    }

    public static IServiceCollection UseAutoRegistrationForAssembly<TTypeFromAssembly>(this IServiceCollection serviceCollection)
    {
        var type = typeof(TTypeFromAssembly);
        var assembly = Assembly.GetAssembly(type);
        if (assembly is null)
        {
            throw new AutoRegistrationException($"Не смогли определить сборку по типу {type}");
        }

        return serviceCollection.UseAutoRegistrationForAssembly(assembly);
    }

    public static IServiceCollection UseAutoRegistrationForAssembly(this IServiceCollection serviceCollection, Assembly assembly)
    {
        var serviceAssemblies = AssemblyProvider.GetServiceAssemblies();

        assembly.GetExportedTypes()
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .SelectMany(
                x => x.GetInterfaces()
                    .Where(i => serviceAssemblies.Contains(i.Assembly))
                    .Select(i => new ServiceDescriptor(i, x, LifestyleByDefault))
            )
            .Select(
                x => x.TryConvertToGenericDefinition(out var descriptorWithGenericDefinition)
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