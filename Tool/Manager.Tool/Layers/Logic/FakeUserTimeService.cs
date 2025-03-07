using System;
using Manager.AuthenticationService.Client.ServiceModels;
using Manager.Core.DateTimeProvider;

namespace Manager.Tool.Layers.Logic;

public class FakeUserTimeService(IDateTimeProvider dateTimeProvider) : IUserTimeService
{
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public DateTime GetUserTime(User user)
    {
        return _dateTimeProvider.Now;
    }
}