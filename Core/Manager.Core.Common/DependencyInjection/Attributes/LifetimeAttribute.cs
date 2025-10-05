using System;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.Common.DependencyInjection.Attributes;

public abstract class LifetimeAttribute(
    ServiceLifetime lifetime
) : Attribute
{
    public readonly ServiceLifetime Lifetime = lifetime;
}