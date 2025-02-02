namespace ManagerService.Client.ServiceModels;

public class TimerRequest
{
    public required User User { get; set; }
    public required string Name { get; set; }
}