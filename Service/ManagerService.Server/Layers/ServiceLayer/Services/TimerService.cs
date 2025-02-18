using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.LinqExtensions;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Services;

public class TimerService(
    ITimerRepository timerRepository,
    ITimerSessionService timerSessionService
) : ITimerService
{
    private readonly ITimerRepository _timerRepository = timerRepository;
    private readonly ITimerSessionService _timerSessionService = timerSessionService;

    //TODO: Создание новой сессии при старте таймера
    public async Task StartTimerAsync(TimerDto timerDto)
    {
        var timer = await _timerRepository.FindAsync(timerDto.UserId, timerDto.Name);
        if (timer is null)
        {
            await _timerRepository.CreateOrUpdateAsync(timerDto);
            await _timerSessionService.StartSessionAsync(timerDto.Id, timerDto.StartTime!.Value);
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
        dtos.Foreach(x =>
            x.Sessions = _timerSessionService
                .SelectByTimerAsync(x.Id)
                .Result.ToArray());
        return dtos;
    }

    public async Task StopTimerAsync(Guid userId, string name, DateTime stopTime)
    {
        var timer = await _timerRepository.FindAsync(userId, name);
        if (timer is null)
        {
            throw new InvalidOperationException($"Timer with name: {name} does not exist.");
        }

        if (timer.Status is not TimerStatus.Stopped)
        {
            throw new InvalidOperationException($"Timer with name: {name} has not stopped.");
        }

        timer.Status = TimerStatus.Reset;
        var timerSessions = await _timerSessionService.SelectByTimerAsync(timer.Id);
        await _timerSessionService.StopTimerSessionAsync(
            timerSessions
                .OrderBy(x => x.StartTime)
                .Last().Id
        );
        await _timerRepository.CreateOrUpdateAsync(timer);
    }

    public async Task ResetTimerAsync(Guid userId, string name)
    {
        var timer = await _timerRepository.FindAsync(userId, name);
        if (timer is null)
        {
            throw new InvalidOperationException($"Timer with name: {name} does not exist.");
        }

        if (timer.Status is not TimerStatus.Started)
        {
            throw new InvalidOperationException($"Timer with name: {name} has not started.");
        }

        var resetTimer = new TimerDto()
        {
            UserId = userId,
            Name = $"{name}_archived",
            StartTime = timer.StartTime,
            PingTimeout = timer.PingTimeout,
            Status = TimerStatus.Reset,
            Sessions = timer.Sessions,
            Id = timer.Id
        };
        await _timerRepository.CreateOrUpdateAsync(resetTimer);
        await CreateAsync(userId, name, timer.PingTimeout);
    }

    public async Task<TimerDto?> FindTimerAsync(Guid userId, string name)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteTimerAsync(Guid userId, string name)
    {
        throw new NotImplementedException();
    }

    private async Task CreateAsync(Guid userId, string name, TimeSpan? pingTimeout)
    {
        var timer = new TimerDto()
        {
            UserId = userId,
            Name = $"{name}",
            StartTime = null,
            PingTimeout = pingTimeout,
            Sessions = Array.Empty<TimerSessionDto>(),
            Id = Guid.NewGuid(),
            Status = TimerStatus.Created
        };
        await _timerRepository.CreateOrUpdateAsync(timer);
    }
}