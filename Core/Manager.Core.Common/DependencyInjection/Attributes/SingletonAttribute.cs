using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.Attributes;

public class SingletonAttribute() : LifetimeAttribute(ServiceLifetime.Singleton);