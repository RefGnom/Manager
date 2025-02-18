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
            Sessions = new TimerSessionDto[]
            {
                new TimerSessionDto()
                {
                    Id = Guid.NewGuid(),
                    TimerId = timerId,
                    StartTime = startTimerRequest.StartTime,
                    StopTime = null,
                    IsOver = false
                }
            },
            Status = TimerStatus.Started
        };
    }
}