using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.DateTimeProvider;
using Manager.Core.DependencyInjection.LifetimeAttributes;
using Manager.Core.Extensions;
using Manager.Core.Extensions.LinqExtensions;
using Manager.TimerService.Client.ServiceModels;
using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;
using Manager.TimerService.Server.Layers.ServiceLayer.Factories;
using Manager.TimerService.Server.ServiceModels;

namespace Manager.TimerService.Server.Layers.ServiceLayer.Services;

[Scoped]
public class TimersService(
    ITimerRepository timerRepository,
    ITimerSessionService timerSessionService,
    IDateTimeProvider dateTimeProvider,
    ITimerDtoFactory timerDtoFactory
) : ITimerService
{
    private readonly ITimerRepository _timerRepository = timerRepository;
    private readonly ITimerSessionService _timerSessionService = timerSessionService;
    private readonly IDateTimeProvider _dateTimeProvider = dateTimeProvider;
    private readonly ITimerDtoFactory _timerDtoFactory = timerDtoFactory;

    public async Task StartAsync(TimerDto timerDto)
    {
        var timer = await _timerRepository.FindAsync(timerDto.UserId, timerDto.Name);
        if (timer is null)
        {
            timerDto.Status = TimerStatus.Started;
            await _timerRepository.CreateAsync(timerDto);
            await _timerSessionService.StartAsync(timerDto.Id, timerDto.StartTime!.Value);
            return;
        }

        if (timer.Status is not (TimerStatus.Stopped or TimerStatus.Reset))
        {
            throw new InvalidStatusException($"Timer cannot started. Timer status: {timer.Status}");
        }

        timer.Status = TimerStatus.Started;
        await _timerRepository.UpdateAsync(timer);
        await _timerSessionService.StartAsync(timer.Id, timerDto.StartTime!.Value);
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

    public async Task StopAsync(Guid userId, string name, DateTime stopTime)
    {
        var timer = await _timerRepository.FindAsync(userId, name);
        if (timer is null)
        {
            throw new NotFoundException($"Timer with name: {name} does not exist.");
        }

        if (timer.Status is not TimerStatus.Started)
        {
            throw new InvalidStatusException(
                $"Остановить таймер можно только в статусе \"{TimerStatus.Started.GetDescription()}\"," +
                $" но был получен в статусе {timer.Status.GetDescription()}"
            );
        }

        timer.Status = TimerStatus.Stopped;
        await _timerRepository.UpdateAsync(timer);
        await _timerSessionService.StopTimerSessionAsync(timer.Id, stopTime);
    }

    public async Task ResetAsync(Guid userId, string name)
    {
        var timer = await _timerRepository.FindAsync(userId, name);
        if (timer is null)
        {
            throw new NotFoundException($"Timer with name: {name} does not exist.");
        }

        if (timer.Status is not TimerStatus.Stopped)
        {
            throw new InvalidStatusException(
                $"Сбросить таймер можно только в статусе \"{TimerStatus.Stopped.GetDescription()}\"," +
                $" но был получен в статусе {timer.Status.GetDescription()}"
            );
        }

        await ArchiveAsync(timer);
        var newTimer = _timerDtoFactory.CreateResetTimer(timer);
        await _timerRepository.CreateAsync(newTimer);
    }

    public Task<TimerDto?> FindAsync(Guid userId, string name)
    {
        return _timerRepository.FindAsync(userId, name);
    }

    public async Task DeleteAsync(Guid userId, string name)
    {
        var timer = await _timerRepository.FindAsync(userId, name);
        if (timer is null)
        {
            throw new NotFoundException($"Timer with name: {name} does not exist.");
        }

        if (timer.Status is not TimerStatus.Stopped)
        {
            throw new InvalidStatusException(
                $"Удалить таймер можно только в статусе \"{TimerStatus.Stopped.GetDescription()}\"," +
                $" но был получен в статусе {timer.Status.GetDescription()}"
            );
        }

        var deletedTimer = _timerDtoFactory.CreateDeletedTimer(timer);
        await _timerRepository.UpdateAsync(deletedTimer);
    }

    public TimeSpan CalculateElapsedTime(TimerDto timerDto) => timerDto.Sessions.Aggregate(
        TimeSpan.Zero,
        (current, session) => current + ((session.StopTime ?? _dateTimeProvider.UtcNow) - session.StartTime)
    );

    public Task ArchiveAsync(TimerDto timerToArchiving)
    {
        var archivedTimer = _timerDtoFactory.CreateArchived(timerToArchiving);
        return _timerRepository.UpdateAsync(archivedTimer);
    }
}