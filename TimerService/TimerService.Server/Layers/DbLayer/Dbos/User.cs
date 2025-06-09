using System;

namespace Manager.TimerService.Server.Layers.DbLayer.Dbos;

public class User
{
    public required Guid Id { get; init; }
    public required string Login { get; init; }
    public required string Email { get; init; }
    public required string Password { get; set; }
}