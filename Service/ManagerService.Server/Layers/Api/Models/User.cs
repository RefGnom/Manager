using System;

namespace ManagerService.Server.Layers.Api.Models;

/// <summary>
/// Информация о пользователе
/// </summary>
public class User
{
    public required Guid Id { get; init; }
}