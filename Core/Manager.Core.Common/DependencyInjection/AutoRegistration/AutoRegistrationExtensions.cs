using System.Linq;
using System.Reflection;
using Manager.Core.Common.DependencyInjection.Attributes;
using Manager.Core.Common.Linq;
using Manager.Core.Common.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.AutoRegistration;

public static class AutoRegistrationExtensions
{
    private const ServiceLifetime LifestyleByDefault = ServiceLifetime.Singleton;

    /// <summary>
    ///     Сканирует текущую сборку и автоматически регистрирует все интерфейсы и их реализации с временем жизни
    ///     LifestyleByDefault.
    ///     Время жизни по умолчанию можно переопределить с помощью атрибутов
    /// </summary>
    public static IServiceCollection UseAutoRegistrationForCurrentAssembly(this IServiceCollection serviceCollection) =>
        serviceCollection.UseAutoRegistrationForAssembly(Assembly.GetCallingAssembly());

    /// <summary>
    ///     Сканирует сборку, в которой находится переданный тип и автоматически регистрирует
    ///     все интерфейсы и их реализации с временем жизни LifestyleByDefault.
    ///     Время жизни по умолчанию можно переопределить с помощью атрибутов
    /// </summary>
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

    /// <summary>
    ///     Регистрирует все зависимости из проекта Manager.Core.Common
    /// </summary>
    public static IServiceCollection UseAutoRegistrationForCoreCommon(
        this IServiceCollection serviceCollection
    )
    {
        var assembly = Assembly.Load("Manager.Core.Common");
        return serviceCollection.UseAutoRegistrationForAssembly(assembly);
    }

    /// <summary>
    ///     Регистрирует все зависимости из проекта Manager.Core.Networking
    /// </summary>
    public static IServiceCollection UseAutoRegistrationForCoreNetworking(
        this IServiceCollection serviceCollection
    )
    {
        var assembly = Assembly.Load("Manager.Core.Networking");
        return serviceCollection.UseAutoRegistrationForAssembly(assembly);
    }

    /// <summary>
    ///     Сканирует переданную сборку и автоматически регистрирует все интерфейсы и их реализации с временем жизни
    ///     LifestyleByDefault.
    ///     Время жизни по умолчанию можно переопределить с помощью атрибутов
    /// </summary>
    /// <param name="serviceCollection">DI collection</param>
    /// <param name="assembly">Сборка, зависимости из которой нужно зарегистрировать</param>
    /// <param name="namespacePrefix">
    ///     Префикс пространства имён.
    ///     Можно использовать, если хотите зарегистрировать зависимости из конкретной папки проекта
    /// </param>
    /// <returns>Configured DI collection</returns>
    public static IServiceCollection UseAutoRegistrationForAssembly(
        this IServiceCollection serviceCollection,
        Assembly assembly,
        string? namespacePrefix = null
    )
    {
        var serviceAssemblies = AssemblyProvider.GetServiceAssemblies();

        assembly.GetExportedTypes()
            .Where(x => !x.IsInterface)
            .Where(x => !x.IsAbstract)
            .Where(implementation => !implementation.HasInterface<IgnoreAutoRegistrationAttribute>())
            .SelectMany(implementation => implementation.GetInterfaces()
                .Where(@interface => serviceAssemblies.Contains(@interface.Assembly))
                .Where(@interface => namespacePrefix is null || @interface.Namespace!.StartsWith(namespacePrefix))
                .Where(@interface => !@interface.HasInterface<IgnoreAutoRegistrationAttribute>())
                .Select(@interface =>
                    {
                        var lifetimeAttribute = implementation.GetCustomAttribute<LifetimeAttribute>();
                        var lifetime = lifetimeAttribute?.Lifetime ?? LifestyleByDefault;
                        return new ServiceDescriptor(@interface, implementation, lifetime);
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