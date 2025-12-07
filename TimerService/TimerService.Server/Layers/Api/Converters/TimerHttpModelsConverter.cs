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
    public TimerDto FromStartRequest(Guid recipientId, string timerName, StartTimerRequest request)
    {
        var timerId = Guid.NewGuid();
        return new TimerDto
        {
            Id = timerId,
            UserId = recipientId,
            Name = timerName,
            StartTime = request.StartTime,
            PingTimeout = request.PingTimeout,
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