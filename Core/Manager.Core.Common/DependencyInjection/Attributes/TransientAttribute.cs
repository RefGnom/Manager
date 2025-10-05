using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.Attributes;

public class TransientAttribute() : LifetimeAttribute(ServiceLifetime.Transient);