using System;
using Manager.Core.DateTimeProvider;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Factories;

public class TimerDtoFactory(IDateTimeProvider dateTimeProvider) : ITimerDtoFactory
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public TimerDto CreateArchived(TimerDto forArchiving)
    {
        return new TimerDto
        {
            Id = forArchiving.Id,
            UserId = forArchiving.UserId,
            Name = $"{forArchiving.Name}_archived_{_dateTimeProvider.Now}",
            StartTime = forArchiving.StartTime,
            PingTimeout = forArchiving.PingTimeout,
            Sessions = forArchiving.Sessions,
            Status = TimerStatus.Archived,
        };
    }

    public TimerDto CreateResetTimer(TimerDto forResetting)
    {
        return new TimerDto
        {
            Id = Guid.NewGuid(),
            UserId = forResetting.UserId,
            Name = forResetting.Name,
            StartTime = null,
            PingTimeout = null,
            Sessions = [],
            Status = TimerStatus.Reset,
        };
    }

    public TimerDto CreateDeletedTimer(TimerDto forDeleting)
    {
        return new TimerDto
        {
            Id = forDeleting.Id,
            UserId = forDeleting.UserId,
            Name = $"{forDeleting.Name}_deleted_{_dateTimeProvider.Now}",
            StartTime = forDeleting.StartTime,
            PingTimeout = forDeleting.PingTimeout,
            Sessions = forDeleting.Sessions,
            Status = TimerStatus.Deleted,
        };
    }
}