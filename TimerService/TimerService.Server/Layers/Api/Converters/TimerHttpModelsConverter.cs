using System;
using System.Linq;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;
using StartTimerRequest = Manager.TimerService.Server.Layers.Api.Models.StartTimerRequest;

namespace Manager.TimerService.Server.Layers.Api.Converters;

public class TimerHttpModelsConverter(
    ITimerSessionHttpModelConverter timerSessionHttpModelConverter
) : ITimerHttpModelsConverter
{
    public TimerDto FromStartRequest(StartTimerRequest startTimerRequest)
    {
        var timerId = Guid.NewGuid();
        return new TimerDto
        {
            Id = timerId,
            UserId = startTimerRequest.UserId,
            Name = startTimerRequest.Name,
            StartTime = startTimerRequest.StartTime,
            PingTimeout = startTimerRequest.PingTimeout,
            Sessions = [],
            Status = TimerStatus.Started,
        };
    }

    public TimerResponse ConvertToTimerResponse(TimerDto timerDto, TimeSpan elapsedTime)
    {
        var sessions = timerDto.Sessions
            .Select(timerSessionHttpModelConverter.ConvertToTimerSessionResponse)
            .ToArray();

        return new TimerResponse
        {
            Name = timerDto.Name,
            StartTime = timerDto.StartTime,
            ElapsedTime = elapsedTime,
            PingTimeout = timerDto.PingTimeout,
            Sessions = sessions,
            TimerStatus = TimerStatus.Created,
        };
    }
}