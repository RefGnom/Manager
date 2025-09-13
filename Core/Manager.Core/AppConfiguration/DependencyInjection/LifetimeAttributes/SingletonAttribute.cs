using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.AppConfiguration.DependencyInjection.LifetimeAttributes;

public class SingletonAttribute() : LifetimeAttribute(ServiceLifetime.Singleton)
{
}