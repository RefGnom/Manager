using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.DependencyInjection.LifetimeAttributes;

public class SingletonAttribute() : LifetimeAttribute(ServiceLifetime.Singleton)
{
}