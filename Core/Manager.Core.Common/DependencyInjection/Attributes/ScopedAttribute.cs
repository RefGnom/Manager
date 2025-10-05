using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.Attributes;

public class ScopedAttribute() : LifetimeAttribute(ServiceLifetime.Scoped);