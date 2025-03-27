using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.ServiceModels;

namespace TimerService.Server.Test.Factories.Extensions;

internal static class TimerDtoExtensions
{
    public static TimerDto WithStatus(this TimerDto timer, TimerStatus status)
    {
        timer.Status = status;
        return timer;
    }

    public static TimerDto WithName(this TimerDto timer, string name)
    {
        timer.Name = name;
        return timer;
    }

    public static TimerDto WithSessions(this TimerDto timer, TimerSessionDto[] sessions)
    {
        timer.Sessions = sessions;
        return timer;
    }

    public static TimerDto WithNewId(this TimerDto timer)
    {
        timer.Id = Guid.NewGuid();
        return timer;
    }
}