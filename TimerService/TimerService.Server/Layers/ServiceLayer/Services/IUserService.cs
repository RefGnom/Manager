using System.Threading.Tasks;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Services;

public interface IUserService
{
    Task CreateUserAsync();
}