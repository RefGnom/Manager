using System;

namespace Manager.Core.Common.DependencyInjection.Attributes;

[AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
public class IgnoreAutoRegistrationAttribute : Attribute;