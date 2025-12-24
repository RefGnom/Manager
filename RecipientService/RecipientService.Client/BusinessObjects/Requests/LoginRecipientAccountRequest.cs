namespace Manager.RecipientService.Client.BusinessObjects.Requests;

public record LoginRecipientAccountRequest(
    string Login,
    string Password
);