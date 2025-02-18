using System;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.Api.Converters;

public interface ITimerHttpModelsConverter
{
    TimerDto FromStartRequest(StartTimerRequest startTimerRequest);
    TimerResponse ConvertToTimerResponse(TimerDto timerDto, TimeSpan elapsedTime);
}