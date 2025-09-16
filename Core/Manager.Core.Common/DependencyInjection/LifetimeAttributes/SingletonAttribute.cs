using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.LifetimeAttributes;

public class SingletonAttribute() : LifetimeAttribute(ServiceLifetime.Singleton)
{
}