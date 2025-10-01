using System;
using Manager.Tool.BusinessObjects;

namespace Manager.Tool.Layers.Logic;

public interface IUserTimeService
{
    DateTime GetUserTime(LocalRecipient localRecipient);
}