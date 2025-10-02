using System;
using Manager.Core.Common.DependencyInjection.LifetimeAttributes;
using Manager.Core.Common.Time;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Factories;

[Transient]
public class TimerDtoFactory(
    IDateTimeProvider dateTimeProvider
) : ITimerDtoFactory
{
    public TimerDto CreateArchived(TimerDto forArchiving) => new()
    {
        Id = forArchiving.Id,
        UserId = forArchiving.UserId,
        Name = $"{forArchiving.Name}_archived_{dateTimeProvider.Now}",
        StartTime = forArchiving.StartTime,
        PingTimeout = forArchiving.PingTimeout,
        Sessions = forArchiving.Sessions,
        Status = TimerStatus.Archived,
    };

    public TimerDto CreateResetTimer(TimerDto forResetting) => new()
    {
        Id = Guid.NewGuid(),
        UserId = forResetting.UserId,
        Name = forResetting.Name,
        StartTime = null,
        PingTimeout = null,
        Sessions = [],
        Status = TimerStatus.Reset,
    };

    public TimerDto CreateDeletedTimer(TimerDto forDeleting) => new()
    {
        Id = forDeleting.Id,
        UserId = forDeleting.UserId,
        Name = $"{forDeleting.Name}_deleted_{dateTimeProvider.Now}",
        StartTime = forDeleting.StartTime,
        PingTimeout = forDeleting.PingTimeout,
        Sessions = forDeleting.Sessions,
        Status = TimerStatus.Deleted,
    };
}