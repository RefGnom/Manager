using System;

namespace Manager.Core.AppConfiguration.Authentication;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationResourceAttribute(string resource) : Attribute
{
    public string Resource { get; } = resource;
}