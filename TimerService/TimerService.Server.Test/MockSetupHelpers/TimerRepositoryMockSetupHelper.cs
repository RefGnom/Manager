using Manager.TimerService.Server.Layers.RepositoryLayer;
using Manager.TimerService.Server.ServiceModels;
using NSubstitute;

namespace TimerService.Server.Test.MockSetupHelpers;

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
}