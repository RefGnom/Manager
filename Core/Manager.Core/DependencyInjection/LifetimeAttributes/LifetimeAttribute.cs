﻿using System;
using Microsoft.Extensions.DependencyInjection;

namespace Manager.Core.DependencyInjection.LifetimeAttributes;

public abstract class LifetimeAttribute(ServiceLifetime lifetime) : Attribute
{
    public readonly ServiceLifetime Lifetime = lifetime;
}