using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic;

public interface IUserProvider
{
    User GetUser();
}