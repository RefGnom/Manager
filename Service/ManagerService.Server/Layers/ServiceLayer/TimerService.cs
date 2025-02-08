using AutoMapper;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public class TimerService(ITimerRepository repository, IMapper mapper) : ITimerService
{
    private readonly ITimerRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task StartTimerAsync(TimerRequest request)
    {
        await _repository.CreateOrUpdateAsync(_mapper.Map<TimerDto>(request));
    }
}