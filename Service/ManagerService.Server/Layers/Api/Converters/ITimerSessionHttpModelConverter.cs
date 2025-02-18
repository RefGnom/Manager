using ManagerService.Client.ServiceModels;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.Api.Converters;

public interface ITimerSessionHttpModelConverter
{
    TimerSessionResponse ConvertToTimerSessionResponse(TimerSessionDto sessionDto);
}