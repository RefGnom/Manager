using System.ComponentModel;
using Manager.Core.Common.Time;
using Manager.ManagerTgClient.Bot.Commands.Attributes;
using Manager.ManagerTgClient.Bot.Commands.Requests;
using Manager.ManagerTgClient.Bot.Commands.Results;
using Manager.ManagerTgClient.Bot.Services;
using Manager.TimerService.Client;
using Manager.TimerService.Client.ServiceModels.Factories;

namespace Manager.ManagerTgClient.Bot.Commands;

[CommandName("/startTimer")]
[Description("Создает таймер")]
public class StartTimerCommand(
    ITimerServiceApiClient serviceApiClient,
    IRequestFactory requestFactory,
    IAuthentificationService authService,
    IDateTimeProvider dateTimeProvider
) : ICommand
{
    public async Task<ICommandResult> ExecuteAsync(ICommandRequest request)
    {
        var startTimerRequest = request as StartTimerRequest;
        var user = await authService.FindUserAsync(startTimerRequest!.ChatId);
        var httpResponse = await serviceApiClient.StartTimerAsync(
            requestFactory.CreateStartTimerRequest(
                user!.RecipientId,
                startTimerRequest.TimerName,
                dateTimeProvider.UtcNow
            )
        );
        return httpResponse.IsSuccessStatusCode
            ? new CommandResult($"Таймер {startTimerRequest.TimerName} успешно создан")
            : new CommandResult($"Не удалось создать таймер. HttpStatusCode: {httpResponse.StatusCode}");
    }

    public string Name => "/startTimer";
}