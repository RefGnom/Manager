using System;
using System.Net;
using System.Threading.Tasks;
using Manager.Core.Common.HelperObjects.Result;
using Manager.RecipientService.Client.BusinessObjects.Requests;
using Manager.RecipientService.Client.BusinessObjects.Responses;

namespace Manager.RecipientService.Client;

public interface IRecipientServiceApiClient
{
    Task<ProcessResult<string, HttpStatusCode>> CreateRecipientAccountAsync(CreateRecipientAccountRequest request);
    Task<RecipientAccountResponse?> FindRecipientAccountAsync(Guid recipientId);
    Task<ProcessResult<string, HttpStatusCode>> DeleteRecipientAccountAsync(Guid recipientId);
    Task<ProcessResult<string, HttpStatusCode>> UpdateRecipientAccountAsync(PatchRecipientAccountRequest request);
    Task<RecipientAuthorizationResponse?> FindRecipientAuthorizationAsync(RecipientAuthorizationRequest request);
}