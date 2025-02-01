using ManagerService.Client.ServiceModels;

namespace Manager.Tool.Layers.Logic;

public class UserProvider : IUserProvider
{
    public User GetUser()
    {
        return new User();
    }
}