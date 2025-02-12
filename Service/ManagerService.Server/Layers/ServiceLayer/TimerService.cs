using AutoMapper;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public class TimerService(
    ITimerRepository repository,
    IMapper mapper
) : ITimerService
{
    private readonly IMapper _mapper = mapper;
    private readonly ITimerRepository _repository = repository;

    public async Task StartTimerAsync(TimerRequest request)
    {
        await _repository.CreateOrUpdateAsync(_mapper.Map<TimerDto>(request));
    }

    public async Task<TimerResponse[]> SelectByUserAsync(Guid userId)
    {
        var dtos = await _repository.SelectByUserAsync(userId);
        return await Task.FromResult(_mapper.Map<TimerResponse[]>(dtos));
    }
}