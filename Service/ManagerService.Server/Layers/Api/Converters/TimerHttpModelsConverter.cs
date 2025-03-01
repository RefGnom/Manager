using System;
using System.Linq;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.Api.Converters;

public class TimerHttpModelsConverter(
    ITimerSessionHttpModelConverter timerSessionHttpModelConverter
) : ITimerHttpModelsConverter
{
    private readonly ITimerSessionHttpModelConverter _timerSessionHttpModelConverter = timerSessionHttpModelConverter;

    public TimerDto FromStartRequest(Models.StartTimerRequest startTimerRequest)
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
            Status = TimerStatus.Started,
        };
    }

    public TimerResponse ConvertToTimerResponse(TimerDto timerDto, TimeSpan elapsedTime)
    {
        var sessions = timerDto.Sessions
            .Select(x => _timerSessionHttpModelConverter.ConvertToTimerSessionResponse(x))
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