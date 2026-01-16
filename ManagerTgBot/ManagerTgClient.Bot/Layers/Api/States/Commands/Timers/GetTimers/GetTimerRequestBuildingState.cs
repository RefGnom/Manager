using Manager.ManagerTgClient.Bot.Layers.Api.Requests.Builders;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Menu;
using Manager.ManagerTgClient.Bot.Layers.Api.States.Templates;
using Manager.ManagerTgClient.Bot.Layers.Services;
using Manager.ManagerTgClient.Bot.Layers.Services.Extentions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace Manager.ManagerTgClient.Bot.Layers.Api.States.Commands.Timers.GetTimers;

public class GetTimerRequestBuildingState(
    IBotInteractionService botInteractionService,
    IStateManager stateManager,
    IGetUserTimersBuilder builder
) : StateBase(botInteractionService, stateManager)
{
    protected override string MessageToSend => $"Выберите фильтры получения таймеров. Затем нажмите готово";
    protected override UpdateType[] SupportedUpdateType => [UpdateType.CallbackQuery];

    protected override InlineKeyboardMarkup InlineKeyboard => new(
        [
            [
                InlineKeyboardButton.WithCallbackData(
                    GetButtonText("Архивные", withArchived),
                    ToggledWithArchivedData
                ),
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    GetButtonText("Удаленные", withDeleted),
                    ToggledWithDeletedData
                ),
            ],
            [
                InlineKeyboardButton.WithCallbackData(
                    "Показать таймеры",
                    ConfirmButtonData
                ),
            ],
        ]
    );

    private const string ToggledWithDeletedData = "withDeleted";
    private const string ToggledWithArchivedData = "withArchived";
    private const string ConfirmButtonData = "/confirm";

    private bool withArchived;
    private bool withDeleted;

    private static string GetButtonText(string text, bool isSelected)
        => isSelected ? $"☑️ {text}" : $"⬜️ {text}";

    protected override async Task HandleUpdateAsync(Update update)
    {
        var userId = update.GetUserId();
        var data = update.GetUserData();
        var messageId = update.GetMessageId();
        switch (data)
        {
            case ToggledWithDeletedData:
            {
                withDeleted = !withDeleted;
                await UpdateKeyboardAsync(userId, messageId);
                break;
            }
            case ToggledWithArchivedData:
            {
                withArchived = !withArchived;
                await UpdateKeyboardAsync(userId, messageId);
                break;
            }
            case ConfirmButtonData:
            {
                builder.ForUser(userId);
                if (withDeleted)
                {
                    builder.WithDeleted();
                }

                if (withArchived)
                {
                    builder.WithArchived();
                }

                var tempTimers = "timers:";
                await BotInteractionService.SendMessageAsync(userId, tempTimers);
                await SetNextStateAsync(userId, new MainMenuState(BotInteractionService, StateManager));
                break;
            }
        }
    }
    private async Task UpdateKeyboardAsync(long chatId, int messageId)
    {
        await BotInteractionService.EditKeyboardAsync(chatId, messageId, InlineKeyboard);
    }
}