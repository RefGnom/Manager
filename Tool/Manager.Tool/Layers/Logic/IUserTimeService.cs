using System;
using Manager.AuthenticationService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic;

public interface IUserTimeService
{
    DateTime GetUserTime(User user);
}