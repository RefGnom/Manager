using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public class TimerService(ITimerRepository repository): ITimerService
{
    private readonly ITimerRepository _repository = repository;
    public async Task StartTimerAsync(User user, string name)
    {
        await _repository.CreateOrUpdateAsync(
            new TimerDto()
        {
            Id = Guid.NewGuid(),
            Name = name,
            UserId = user.Id,
            StartTime = DateTime.Now,
            PingTimeout = null,
            Status = TimerStatus.Started
        });
    }
}