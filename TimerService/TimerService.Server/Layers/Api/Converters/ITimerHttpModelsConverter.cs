using System;
using Manager.TimerService.Server.Layers.Api.Models;
using Manager.TimerService.Server.ServiceModels;
using TimerResponse = Manager.TimerService.Client.ServiceModels.TimerResponse;

namespace Manager.TimerService.Server.Layers.Api.Converters;

public interface ITimerHttpModelsConverter
{
    TimerDto FromStartRequest(Guid recipientId, string timerName, StartTimerRequest startTimerRequest);
    TimerResponse ConvertToTimerResponse(TimerDto timerDto, TimeSpan elapsedTime);
}