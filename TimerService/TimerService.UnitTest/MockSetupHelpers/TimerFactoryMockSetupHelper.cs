using Manager.TimerService.Server.Layers.ServiceLayer.Factories;
using Manager.TimerService.Server.ServiceModels;
using NSubstitute;

namespace Manager.TimerService.UnitTest.MockSetupHelpers;

public static class TimerFactoryMockSetupHelper
{
    public static void ConfigureCreateArchivedTimer(
        this ITimerDtoFactory factory,
        TimerDto timer,
        TimerDto archivedTimer
    ) =>
        factory.CreateArchived(timer)
            .Returns(archivedTimer);

    public static void ConfigureCreateResetTimer(
        this ITimerDtoFactory factory,
        TimerDto timer,
        TimerDto resetTimer
    ) =>
        factory.CreateResetTimer(timer)
            .Returns(timer);

    public static void ConfigureCreateDeletedTimer(
        this ITimerDtoFactory factory,
        TimerDto timer,
        TimerDto archivedTimer
    ) =>
        factory.CreateDeletedTimer(timer)
            .Returns(archivedTimer);
}