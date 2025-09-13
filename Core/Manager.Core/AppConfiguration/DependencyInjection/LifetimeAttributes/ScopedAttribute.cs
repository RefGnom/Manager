using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.AppConfiguration.DependencyInjection.LifetimeAttributes;

public class ScopedAttribute() : LifetimeAttribute(ServiceLifetime.Scoped)
{
}