using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.LifetimeAttributes;

public class TransientAttribute() : LifetimeAttribute(ServiceLifetime.Transient)
{
}