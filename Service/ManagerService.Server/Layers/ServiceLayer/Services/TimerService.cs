using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.DateTimeProvider;
using Manager.Core.LinqExtensions;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Services;

public class TimerService(
    ITimerRepository timerRepository,
    ITimerSessionService timerSessionService,
    IDateTimeProvider dateTimeProvider
) : ITimerService
{
    private readonly ITimerRepository _timerRepository = timerRepository;
    private readonly ITimerSessionService _timerSessionService = timerSessionService;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;

    public async Task StartTimerAsync(TimerDto timerDto)
    {
        var timer = await _timerRepository.FindAsync(timerDto.UserId, timerDto.Name);
        if (timer is null)
        {
            timer = timerDto;
        }
        else
        {
            if (timer.Status is not (TimerStatus.Stopped or TimerStatus.Reset))
            {
                throw new InvalidOperationException($"Timer cannot started. Timer status: {timer.Status}");
            }

            timer.StartTime = timerDto.StartTime;
            timer.PingTimeout = timerDto.PingTimeout;
        }

        timer.Status = TimerStatus.Started;
        await _timerRepository.CreateOrUpdateAsync(timer);
        await _timerSessionService.StartSessionAsync(timer.Id, timer.StartTime!.Value);
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

        await dtos.ForeachAsync(
            async x => x.Sessions = await _timerSessionService.SelectByTimerAsync(x.Id)
        );
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
        await _timerSessionService.StopTimerSessionAsync(timer.Id, stopTime);
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

    public TimeSpan CalculateElapsedTime(TimerDto timerDto) => timerDto.Sessions.Aggregate(
        TimeSpan.Zero,
        (current, session) => current + ((session.StopTime ?? _dateTimeProvider.Now) - session.StartTime)
    );

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