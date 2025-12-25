namespace Manager.RecipientService.Server.Dao.Api.Responses;

public record ErrorResponse(
    string Code,
    string Message
)
{
    public static ErrorResponse Create(string code, string message) => new(code, message);
};