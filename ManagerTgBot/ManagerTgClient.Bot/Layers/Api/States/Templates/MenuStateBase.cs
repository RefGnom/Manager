using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;

public abstract class MenuStateBase(
    IBotInteractionService botInteractionService,
    IStateManager stateManager
) : StateBase(botInteractionService, stateManager)
{
    protected abstract Dictionary<string, Type> States { get; }

    protected override async Task HandleUpdateAsync(Update update)
    {
        var userData = update.GetUserData()!;
        var userId = update.GetUserId();
        if (!States.TryGetValue(userData, out var state))
        {
            return;
        }

        await StateManager.SetStateAsync(userId, state);
    }
}