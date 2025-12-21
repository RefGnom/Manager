using System;
using System.Threading.Tasks;
using Manager.WorkService.Client.Requests;
using Manager.WorkService.Client.Responses;

namespace Manager.WorkService.Client;

public interface IWorkServiceApiClient
{
    Task<Guid> CreateWorkAsync(CreateWorkRequest createWorkRequest);
    Task<GetWorkResponse?> FindWorkAsync(Guid recipientId, Guid workId);
    Task<GetWorkResponse[]> SelectWorksAsync(Guid recipientId);
    Task<GetWorkResponse[]> SelectActualWorksAsync(Guid recipientId);
    Task<GetWorkResponse[]> SelectWorksForReminderAsync(Guid recipientId);
    Task<GetWorkResponse[]> SelectExpiredWorksAsync(Guid recipientId);
    Task<GetWorkResponse[]> SelectDeletedWorksAsync(Guid recipientId);
    Task<GetWorkResponse[]> SelectCompletedWorksAsync(Guid recipientId);
    Task UpdateWorkAsync(UpdateWorkRequest updateWorkRequest);
    Task DeleteWorkAsync(Guid recipientId, Guid workId);
}