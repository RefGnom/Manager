using System;
using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.Server.ServiceModels;
using NSubstitute;

namespace Manager.TimerService.UnitTest.MockSetupHelpers;

public static class TimerRepositoryMockSetupHelper
{
    public static void ConfigureFindMethod(this ITimerRepository repository, TimerDto timerDto) =>
        repository.ConfigureFindMethod(timerDto.UserId, timerDto.Name, timerDto);

    public static void ConfigureFindMethod(
        this ITimerRepository repository,
        Guid userId,
        string timerName,
        TimerDto returnTimer
    ) =>
        repository
            .FindAsync(userId, timerName)
            .Returns(returnTimer);

    public static void ConfigureSelectByUserMethod(
        this ITimerRepository repository,
        Guid userId,
        TimerDto[] resultTimers
    ) => repository
        .SelectByUserAsync(userId)
        .Returns(resultTimers);
}