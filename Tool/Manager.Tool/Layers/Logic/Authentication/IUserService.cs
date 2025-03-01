using System;
using System.Threading.Tasks;
using Manager.AuthenticationService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic.Authentication;

public interface IUserService
{
    User? FindUser();
    Task SaveUserIdAsync(Guid userId);
}