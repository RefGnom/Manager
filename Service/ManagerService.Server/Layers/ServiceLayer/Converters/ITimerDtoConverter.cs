using ManagerService.Client.ServiceModels;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Convertors;

public interface ITimerDtoConverter
{
    TimerDto FromStartRequest(StartTimerRequest startTimerRequest);
}