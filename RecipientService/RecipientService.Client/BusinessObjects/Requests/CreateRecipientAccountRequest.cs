namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public record CreateRecipientAccountRequest(
    string Login,
    string Password,
    int RecipientTimeUtcOffsetHours
);