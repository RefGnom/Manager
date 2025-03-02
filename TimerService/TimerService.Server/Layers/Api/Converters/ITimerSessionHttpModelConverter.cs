using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.Api.Converters;

public interface ITimerSessionHttpModelConverter
{
    TimerSessionResponse ConvertToTimerSessionResponse(TimerSessionDto sessionDto);
}