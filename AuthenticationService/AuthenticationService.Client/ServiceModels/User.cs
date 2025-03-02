using System;

namespace Manager.AuthenticationService.Client.ServiceModels;

public class User
{
    public Guid Id { get; set; }
    public TimeSpan? BaseUtcOffset { get; set; }
}