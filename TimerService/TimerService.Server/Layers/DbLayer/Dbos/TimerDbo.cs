﻿using System;
using Manager.TimerService.Client.ServiceModels;

namespace Manager.TimerService.Server.Layers.DbLayer.Dbos;

public class TimerDbo
{
    public required Guid Id { get; init; }
    public required Guid UserId { get; init; }
    public required string Name { get; init; }
    public required DateTime StartTime { get; init; }
    public required TimeSpan? PingTimeout { get; init; }
    public required TimerStatus Status { get; init; }
}