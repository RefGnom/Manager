using System;
using Manager.Core.Common.Time;
using Manager.Tool.BusinessObjects;

namespace Manager.Tool.Layers.Logic;

public class FakeUserTimeService(IDateTimeProvider dateTimeProvider) : IUserTimeService
{
    public DateTime GetUserTime(LocalRecipient localRecipient)
    {
        return dateTimeProvider.Now;
    }
}