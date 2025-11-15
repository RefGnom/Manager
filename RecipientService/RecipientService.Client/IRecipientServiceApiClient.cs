using System;
using System.Threading.Tasks;
using Manager.Core.ApiClientBase;
using Manager.RecipientService.Client.BusinessObjects.Requests;
using Manager.RecipientService.Client.BusinessObjects.Responses;

namespace Manager.RecipientService.Client;

public interface IRecipientServiceApiClient
{
    Task<HttpResult> CreateRecipientAccountAsync(CreateRecipientAccountRequest request);
    Task<HttpResult<RecipientAccountResponse>> GetRecipientAccountAsync(Guid recipientId);
    Task<HttpResult> DeleteRecipientAccountAsync(Guid recipientId);
    Task<HttpResult> UpdateRecipientAccountAsync(PatchRecipientAccountRequest request);

    Task<HttpResult<RecipientAuthorizationResponse>> GetRecipientAuthorizationAsync(
        RecipientAuthorizationRequest request
    );
}