using Manager.RecipientService.Server.Dao.Api.Requests;
using Manager.RecipientService.Server.Dao.Api.Responses;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Dao.Api;

public interface IRecipientAuthorizationConverter
{
    RecipientAuthorizationResponse ToResponse(
        RecipientAuthorizationRequest request,
        RecipientAuthorizationStatus status
    );
}

public class RecipientAuthorizationConverter : IRecipientAuthorizationConverter
{
    public RecipientAuthorizationResponse ToResponse(
        RecipientAuthorizationRequest request,
        RecipientAuthorizationStatus status
    ) => new(
        request.RecipientId,
        request.RequestedService,
        request.RequestedResource,
        status
    );
}