using System;
using System.Threading.Tasks;
using Manager.RecipientService.Server.Implementation.Domain;

namespace Manager.RecipientService.Server.Implementation;

public interface IRecipientAuthorizationService
{
    Task<RecipientAuthorizationStatus?> FindRecipientAuthorizationAsync(
        Guid recipientId,
        string requestedService,
        string requestedResource
    );
}

public class RecipientAuthorizationService : IRecipientAuthorizationService
{
    public Task<RecipientAuthorizationStatus?> FindRecipientAuthorizationAsync(
        Guid recipientId,
        string requestedService,
        string requestedResource
    ) => Task.FromResult<RecipientAuthorizationStatus?>(RecipientAuthorizationStatus.Success);
}