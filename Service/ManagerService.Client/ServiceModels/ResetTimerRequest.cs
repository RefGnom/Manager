namespace ManagerService.Client.ServiceModels;

public class ResetTimerRequest
{
    public required User User { get; set; }
    public required string Name { get; set; }
}