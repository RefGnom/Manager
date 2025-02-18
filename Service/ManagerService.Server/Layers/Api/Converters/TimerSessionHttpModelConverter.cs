using ManagerService.Client.ServiceModels;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.Api.Converters;

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