using System;

namespace Manager.TimerService.Client.ServiceModels;

public record UserTimersRequest(Guid UserId, bool WithArchived, bool WithDeleted);