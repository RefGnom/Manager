﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.DateTimeProvider;
using Manager.Core.DependencyInjection.LifetimeAttributes;
using Manager.Core.Extensions;
using Manager.Core.Extensions.LinqExtensions;
using ManagerService.Client.ServiceModels;
using ManagerService.Server.Layers.RepositoryLayer;
using ManagerService.Server.Layers.ServiceLayer.Exceptions;
using ManagerService.Server.Layers.ServiceLayer.Factories;
using ManagerService.Server.ServiceModels;

namespace ManagerService.Server.Layers.ServiceLayer.Services;

[Scoped]
public class TimerService(
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
        // await _timerSessionService.StartSessionAsync(timer.Id, timer.StartTime!.Value);
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
        await _timerRepository.CreateOrUpdateAsync(timer);
        await _timerSessionService.StopTimerSessionAsync(timer.Id, stopTime);
    }

    public async Task ResetTimerAsync(Guid userId, string name)
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

        await ArchiveTimerAsync(timer);
        var newTimer = _timerDtoFactory.CreateResetTimer(timer);
        await _timerRepository.CreateOrUpdateAsync(newTimer);
    }

    public Task<TimerDto?> FindTimerAsync(Guid userId, string name)
    {
        return _timerRepository.FindAsync(userId, name);
    }

    public async Task DeleteTimerAsync(Guid userId, string name)
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

        var newTimer = _timerDtoFactory.CreateDeletedTimer(timer);
        await _timerRepository.CreateOrUpdateAsync(newTimer);
    }

    public TimeSpan CalculateElapsedTime(TimerDto timerDto) => timerDto.Sessions.Aggregate(
        TimeSpan.Zero,
        (current, session) => current + ((session.StopTime ?? _dateTimeProvider.Now) - session.StartTime)
    );

    public Task ArchiveTimerAsync(TimerDto timerToArchiving)
    {
        var archivedTimer = _timerDtoFactory.CreateArchived(timerToArchiving);
        return _timerRepository.CreateOrUpdateAsync(archivedTimer);
    }
}