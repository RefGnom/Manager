using System;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Convertors;

public class TimerDtoConverter: ITimerDtoConverter
{
    public TimerDto FromStartRequest(StartTimerRequest startTimerRequest)
    {
        var timerId = Guid.NewGuid();
        return new TimerDto()
        {
            Id = timerId,
            UserId = startTimerRequest.User.Id,
            Name = startTimerRequest.Name,
            StartTime = startTimerRequest.StartTime,
            PingTimeout = startTimerRequest.PingTimeout,
            Sessions = [],
            Status = TimerStatus.Started
        };
    }
}