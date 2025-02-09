namespace ManagerService.Client.ServiceModels;

public class DeleteTimerRequest
{
    public required User User { get; set; }
    public required string Name { get; set; }
}