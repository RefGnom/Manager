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

    public async Task StartTimerAsync(StartTimerRequest request)
    {
        var timer = await timerRepository.FindAsync(request.User.Id, request.Name);
        if (timer is null)
        {
            await _timerRepository.CreateOrUpdateAsync(_mapper.Map<TimerDto>(request));
        }
        else
        {
            if (timer.Status is TimerStatus.Started or TimerStatus.Archived or TimerStatus.Deleted)
            {
                throw new Exception($"Timer {timer.Status}");
            }
            timer.StartTime = request.StartTime;
            timer.PingTimeout = request.PingTimeout;
            await _timerRepository.CreateOrUpdateAsync(timer);
        }
    }

    public async Task<TimerResponse[]> SelectByUserAsync(UserTimersRequest request)
    {
        var dtos = await _timerRepository.SelectByUserAsync(request.User.Id);
        if (!request.WithArchived)
        {
            dtos = dtos
                .Where(x => x.Status != TimerStatus.Archived)
                .ToArray();
        }

        if (!request.WithDeleted)
        {
            dtos = dtos
                .Where(x => x.Status != TimerStatus.Deleted)
                .ToArray();
        }

        dtos.Foreach(x => x.Sessions = _timerSessionRepository.SelectByTimer(x.Id).Result);
        return _mapper.Map<TimerResponse[]>(dtos);
    }
}