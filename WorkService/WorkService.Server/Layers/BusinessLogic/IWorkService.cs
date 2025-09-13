using System;
using System.Threading.Tasks;
using Manager.WorkService.Server.Layers.BusinessLogic.Models;

namespace WorkService.Server.Layers.BusinessLogic;

public interface IWorkService
{
    Task CreateWorkAsync(WorkDto workDto);
    Task UpdateWorkAsync(WorkDto workDto);
    Task<WorkDto?> FindWorkAsync(Guid workId);
    Task DeleteWorkAsync(WorkDto workDto);
    Task<WorkDto[]> SelectWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectActualWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectWorksForReminderAsync(Guid recipientId);
    Task<WorkDto[]> SelectExpiredWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectDeletedWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectCompletedWorksAsync(Guid recipientId);
}