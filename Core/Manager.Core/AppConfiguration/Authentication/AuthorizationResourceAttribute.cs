using System;

namespace Manager.Core.AppConfiguration.Authentication;

/// <summary>
///     Атрибут представляющий ресурс авторизации для эндпоинта.
///     Атрибут на методе контроллера переопределяет общий ресурс для контроллера
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizationResourceAttribute(
    string resource
) : Attribute
{
    public string Resource { get; } = resource;
}