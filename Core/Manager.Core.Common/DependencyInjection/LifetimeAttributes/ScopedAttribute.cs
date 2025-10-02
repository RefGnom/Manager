using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.LifetimeAttributes;

public class ScopedAttribute() : LifetimeAttribute(ServiceLifetime.Scoped) { }