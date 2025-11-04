using Manager.Core.Common.Time;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests;
using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Results;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels.Factories;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands;

public class StartTimerCommand(
    ITimerServiceApiClient serviceApiClient,
    IRequestFactory requestFactory,
    IAuthentificationService authService,
    IDateTimeProvider dateTimeProvider
) : CommandBase
{
    public override string Name => "/startTimer";
    public override string Description => "Запускает существующий таймер или создает новый";

    public override async Task<ICommandResult> ExecuteAsync(ICommandRequest commandRequest)
    {
        var request = CastRequest<StartTimerRequest>(commandRequest);
        var user = await authService.FindUserAsync(request!.UserId);
        var httpResponse = await serviceApiClient.StartTimerAsync(
            requestFactory.CreateStartTimerRequest(
                user!.RecipientId,
                request.TimerName,
                dateTimeProvider.UtcNow
            )
        );
        return httpResponse.IsSuccessStatusCode
            ? new CommandResult($"Таймер {request.TimerName} успешно создан")
            : new CommandResult($"Не удалось создать таймер. HttpStatusCode: {httpResponse.StatusCode}");
    }
}