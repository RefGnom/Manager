namespace Manager.RecipientService.Client.BusinessObjects.Responses;

public record LoginRecipientAccountResponse(
    bool CanLogin,
    LoginRecipientAccountErrorStatus? ErrorStatus,
    string? RejectMessage
);