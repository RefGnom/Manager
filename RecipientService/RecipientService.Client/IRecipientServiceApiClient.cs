using System;
using System.Threading.Tasks;
using Manager.Core.Networking;
using Manager.RecipientService.Client.BusinessObjects.Requests;
using Manager.RecipientService.Client.BusinessObjects.Responses;

namespace Manager.RecipientService.Client;

public interface IRecipientServiceApiClient
{
    Task<HttpResult<RecipientAccountResponse>> CreateRecipientAccountAsync(CreateRecipientAccountRequest request);
    Task<HttpResult> LoginRecipientAccountAsync(LoginRecipientAccountRequest request);
    Task<HttpResult<RecipientAccountResponse>> GetRecipientAccountAsync(Guid recipientId);
    Task<HttpResult> DeleteRecipientAccountAsync(Guid recipientId);
    Task<HttpResult> UpdateRecipientAccountAsync(PatchRecipientAccountRequest request);

    Task<HttpResult<RecipientAuthorizationResponse>> GetRecipientAuthorizationAsync(
        RecipientAuthorizationRequest request
    );
}