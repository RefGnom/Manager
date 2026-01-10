using Manager.ManagerTgClient.Bot.Layers.Api.Commands.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.Exceptions;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace Manager.ManagerTgClient.Bot.Layers.Api.Commands.States.Commands.StartTimer;

public class EnteringTimerNameForStartState(
    ITelegramBotClient botClient,
    IStateManager stateManager,
    IStartTimerRequestBuilder builder
) : StateBase(botClient, stateManager)
{
    private readonly IStateManager stateManager = stateManager;
    private readonly ITelegramBotClient botClient = botClient;
    protected override string MessageToSend => "Введите название своего таймера";

    public override async Task ProcessUpdateAsync(Update update)
    {
        if (!IsSupportedUpdate(update))
        {
            throw new NotSupportedUpdateTypeException(update.Type.ToString());
        }

        var userId = update.Message!.From!.Id;
        builder.ForUser(userId).WithName(update.Message!.Text!);
        await SetNextStateAsync(
            userId,
            new EnteringTimerDescriptionState(botClient, stateManager, builder)
        );
    }
}