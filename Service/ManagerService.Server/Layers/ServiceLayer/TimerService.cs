using AutoMapper;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.ServiceModels;
using Manager.Core.LinqExtensions;


namespace ManagerService.Server.Layers.ServiceLayer;

public class TimerService(
    ITimerRepository timerRepository,
    ITimerSessionRepository timerSessionRepository,
    IMapper mapper
) : ITimerService
{
    private readonly IMapper _mapper = mapper;
    private readonly ITimerRepository _timerRepository = timerRepository;
    private readonly ITimerSessionRepository _timerSessionRepository = timerSessionRepository;

    public async Task StartTimerAsync(TimerRequest request)
    {
        await _timerRepository.CreateOrUpdateAsync(_mapper.Map<TimerDto>(request));
    }

    public async Task<TimerResponse[]> SelectByUserAsync(Guid userId)
    {
        var dtos = await _timerRepository.SelectByUserAsync(userId);
        dtos.Foreach(x => x.Sessions = _timerSessionRepository.SelectByTimer(x.Id).Result);
        return _mapper.Map<TimerResponse[]>(dtos);
    }
}