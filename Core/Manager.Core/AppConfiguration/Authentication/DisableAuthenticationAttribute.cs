using System;

namespace Manager.Core.AppConfiguration.Authentication;

/// <summary>
/// Не использовать. Нужен для служебных эндпоинтов
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
internal class DisableAuthenticationAttribute : Attribute;