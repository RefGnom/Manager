namespace ManagerService.Client.ServiceModels;

public class UserTimersRequest
{
    public required User User { get; set; }
    public required bool WithArchived { get; set; }
    public required bool WithDeleted { get; set; }
}