namespace Manager.Tool.BusinessObjects;

public class LocalRecipientWithCredentials : LocalRecipient
{
    public required string Login { get; init; }
    public required string Password { get; init; }
}