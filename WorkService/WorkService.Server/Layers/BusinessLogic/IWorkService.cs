using System;
using System.Threading.Tasks;
using WorkService.Server.Layers.BusinessLogic.Models;

namespace WorkService.Server.Layers.BusinessLogic;

public interface IWorkService
{
    Task CreateWorkAsync(WorkDto workDto);
    Task UpdateWorkAsync(WorkDto workDto);
    Task<WorkDto?> FindWorkAsync(Guid workId);
    Task DeleteWorkAsync(Guid workId);
    Task<WorkDto[]> SelectWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectActualWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectWorksForReminderAsync(Guid recipientId);
    Task<WorkDto[]> SelectExpiredWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectDeletedWorksAsync(Guid recipientId);
    Task<WorkDto[]> SelectCompletedWorksAsync(Guid recipientId);
}

public class WorkService : IWorkService
{
    public Task CreateWorkAsync(WorkDto workDto)
    {
        return Task.CompletedTask;
    }

    public Task UpdateWorkAsync(WorkDto workDto)
    {
        return Task.CompletedTask;
    }

    public Task<WorkDto?> FindWorkAsync(Guid workId)
    {
        return Task.FromResult<WorkDto?>(null);
    }

    public Task DeleteWorkAsync(Guid workId)
    {
        return Task.CompletedTask;
    }

    public Task<WorkDto[]> SelectWorksAsync(Guid recipientId)
    {
        return Task.FromResult(Array.Empty<WorkDto>());
    }

    public Task<WorkDto[]> SelectActualWorksAsync(Guid recipientId)
    {
        return Task.FromResult(Array.Empty<WorkDto>());
    }

    public Task<WorkDto[]> SelectWorksForReminderAsync(Guid recipientId)
    {
        return Task.FromResult(Array.Empty<WorkDto>());
    }

    public Task<WorkDto[]> SelectExpiredWorksAsync(Guid recipientId)
    {
        return Task.FromResult(Array.Empty<WorkDto>());
    }

    public Task<WorkDto[]> SelectDeletedWorksAsync(Guid recipientId)
    {
        return Task.FromResult(Array.Empty<WorkDto>());
    }

    public Task<WorkDto[]> SelectCompletedWorksAsync(Guid recipientId)
    {
        return Task.FromResult(Array.Empty<WorkDto>());
    }
}