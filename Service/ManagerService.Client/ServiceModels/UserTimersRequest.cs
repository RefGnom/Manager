using System;

namespace ManagerService.Client.ServiceModels;

public class UserTimersRequest
{
    public required Guid UserId { get; set; }
    public required bool WithArchived { get; set; }
    public required bool WithDeleted { get; set; }
}