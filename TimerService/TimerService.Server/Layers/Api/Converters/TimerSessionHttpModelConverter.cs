using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.Api.Converters;

public class TimerSessionHttpModelConverter : ITimerSessionHttpModelConverter
{
    public TimerSessionResponse ConvertToTimerSessionResponse(TimerSessionDto sessionDto)
    {
        return new TimerSessionResponse
        {
            StartTime = sessionDto.StartTime,
            StopTime = sessionDto.StopTime,
            IsOver = sessionDto.IsOver,
        };
    }
}