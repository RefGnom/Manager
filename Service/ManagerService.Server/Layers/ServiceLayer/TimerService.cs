using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using Manager.Core.LinqExtensions;
using ManagerService.Server.Convertors;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer;

public class TimerService(
    ITimerRepository timerRepository,
    ITimerSessionRepository timerSessionRepository,
    IMapper mapper,
    ITimerDtoConverter timerDtoConverter
) : ITimerService
{
    private readonly ITimerRepository _timerRepository = timerRepository;
    private readonly ITimerSessionRepository _timerSessionRepository = timerSessionRepository;

    //TODO: Создание новой сессии при старте таймера
    public async Task StartTimerAsync(TimerDto timerDto)
    {
        var timer = await _timerRepository.FindAsync(timerDto.UserId, timerDto.Name);
        if (timer is null)
        {
            await _timerRepository.CreateOrUpdateAsync(timerDto);
        }
        else
        {
            if (timer.Status is TimerStatus.Stopped or TimerStatus.Reset)
            {
                timer.StartTime = timerDto.StartTime;
                timer.PingTimeout = timerDto.PingTimeout;
                await _timerRepository.CreateOrUpdateAsync(timer);
            }

            throw new InvalidOperationException($"Timer cannot started. Timer status: {timer.Status}");
        }
    }

    public async Task<TimerDto[]> SelectByUserAsync(Guid userId, bool withArchived, bool withDeleted)
    {
        var dtos = await _timerRepository.SelectByUserAsync(userId);
        if (!withArchived)
        {
            dtos = dtos
                .Where(x => x.Status != TimerStatus.Archived)
                .ToArray();
        }

        if (!withDeleted)
        {
            dtos = dtos
                .Where(x => x.Status != TimerStatus.Deleted)
                .ToArray();
        }

        //TODO: Подсчёт ElapsedTime на основе сессий.
        dtos.Foreach(x => x.Sessions = _timerSessionRepository.SelectByTimer(x.Id).Result);
        return dtos;
    }
}