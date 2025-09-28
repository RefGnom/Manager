using System;

namespace Manager.Core.AppConfiguration.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DisableAuthenticationAttribute : Attribute;