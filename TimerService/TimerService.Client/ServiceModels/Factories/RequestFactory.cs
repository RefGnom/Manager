using System;

namespace Manager.TimerService.Client.ServiceModels.Factories;

public class RequestFactory : IRequestFactory
{
    public StartTimerRequest CreateStartTimerRequest(
        Guid userId,
        string name,
        DateTime startTime,
        TimeSpan? pingTimeout = null
    ) => new (userId, name, startTime, pingTimeout);
}