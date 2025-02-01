using System;
using System.Threading.Tasks;
using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Authentication;

public interface IUserService
{
    bool TryGetUser(out User user);
    Task SaveUserIdAsync(Guid userId);
}