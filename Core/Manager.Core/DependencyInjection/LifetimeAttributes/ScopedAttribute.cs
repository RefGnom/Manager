using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.DependencyInjection.LifetimeAttributes;

public class ScopedAttribute() : LifetimeAttribute(ServiceLifetime.Scoped)
{
}