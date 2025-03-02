namespace Manager.TimerService.Client.ServiceModels;

public class UserTimersResponse
{
    public required TimerResponse[] Timers { get; set; }
}