using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;

public abstract class ChoiceStateBase(
    IBotInteractionService botInteractionService,
    IStateManager stateManager
) : StateBase(botInteractionService, stateManager)
{
    private const string PositiveAnswer = "Yes";
    private const string NegativeAnswer = "No";

    protected override InlineKeyboardMarkup? InlineKeyboard =>
        new InlineKeyboardMarkup()
            .AddButton("Да", PositiveAnswer)
            .AddButton("Нет", NegativeAnswer);

    protected abstract override string MessageToSend { get; }
    protected abstract override Task HandleUpdateAsync(Update update);
    protected static bool IsPositiveAnswer(Update update) => update.GetUserData()!.Equals(PositiveAnswer);
}