using System;
using Manager.AuthenticationService.Client.ServiceModels;
using Manager.Core.Common.Time;

namespace Manager.Tool.Layers.Logic;

public class FakeUserTimeService(IDateTimeProvider dateTimeProvider) : IUserTimeService
{
    public DateTime GetUserTime(User user)
    {
        return dateTimeProvider.Now;
    }
}