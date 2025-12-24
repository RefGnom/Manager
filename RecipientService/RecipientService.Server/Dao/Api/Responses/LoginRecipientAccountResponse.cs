namespace Manager.RecipientService.Server.Dao.Api.Responses;

public record LoginRecipientAccountResponse(
    bool CanLogin,
    LoginRecipientAccountErrorStatus? ErrorStatus,
    string? RejectMessage
)
{
    public static LoginRecipientAccountResponse CreateSuccess() => new(true, null, null);

    public static LoginRecipientAccountResponse CreateNotFound(string message)
        => new(false, LoginRecipientAccountErrorStatus.NotFound, message);

    public static LoginRecipientAccountResponse CreateDeleted(string message)
        => new(false, LoginRecipientAccountErrorStatus.Deleted, message);

    public static LoginRecipientAccountResponse CreateRejected(string message)
        => new(false, LoginRecipientAccountErrorStatus.Rejected, message);
}