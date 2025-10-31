using System;

namespace Manager.TimerService.Client.ServiceModels.Factories;

public interface IRequestFactory
{
    StartTimerRequest CreateStartTimerRequest(
        Guid userId,
        string name,
        DateTime startTime,
        TimeSpan? pingTimeout = null
    );
}