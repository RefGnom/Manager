using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.AppConfiguration.DependencyInjection.LifetimeAttributes;

public class TransientAttribute() : LifetimeAttribute(ServiceLifetime.Transient)
{
}