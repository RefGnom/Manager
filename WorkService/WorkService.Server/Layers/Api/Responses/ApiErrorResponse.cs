using Manager.Core.Extensions;

namespace Manager.WorkService.Server.Layers.Api.Responses;

public class ApiErrorResponse
{
    private ApiErrorResponse(string errorCode, string errorCodeDescription, string message)
    {
        ErrorCode = errorCode;
        Message = message;
        ErrorCodeDescription = errorCodeDescription;
    }

    public string ErrorCode { get; init; }
    public string ErrorCodeDescription { get; init; }
    public string Message { get; init; }

    public static ApiErrorResponse Create(ApiErrorCode errorCode, string message)
    {
        return new ApiErrorResponse(errorCode.ToString(), errorCode.GetDescription(), message);
    }
}