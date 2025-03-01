using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.DependencyInjection.LifetimeAttributes;

public class TransientAttribute() : LifetimeAttribute(ServiceLifetime.Transient)
{
}