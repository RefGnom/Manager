using System;
using System.Linq;
using System.Threading.Tasks;
using WorkService.Server.Layers.BusinessLogic.Converters;
using WorkService.Server.Layers.BusinessLogic.Models;
using WorkService.Server.Layers.Repository;

namespace WorkService.Server.Layers.BusinessLogic;

public class WorkService(
    IWorkRepository workRepository,
    IWorkConverter workConverter
) : IWorkService
{
    public Task CreateWorkAsync(WorkDto workDto)
    {
        return workRepository.CreateAsync(workConverter.ToDbo(workDto));
    }

    public Task UpdateWorkAsync(WorkDto workDto)
    {
        return workRepository.UpdateAsync(workConverter.ToDbo(workDto));
    }

    public async Task<WorkDto?> FindWorkAsync(Guid workId)
    {
        var workDbo = await workRepository.FindAsync(workId);
        return workDbo is null ? null : workConverter.ToDto(workDbo);
    }

    public Task DeleteWorkAsync(WorkDto workDto)
    {
        var workDtoToUpdate = workDto with { WorkStatus = WorkStatus.Deleted };
        return workRepository.UpdateAsync(workConverter.ToDbo(workDtoToUpdate));
    }

    public async Task<WorkDto[]> SelectWorksAsync(Guid recipientId)
    {
        var workDbos = await workRepository.SelectAsync(recipientId);
        return workDbos.Select(workConverter.ToDto).ToArray();
    }

    public async Task<WorkDto[]> SelectActualWorksAsync(Guid recipientId)
    {
        var workDbos = await workRepository.SelectAsync(recipientId, WorkStatus.Actual);
        return workDbos.Select(workConverter.ToDto).ToArray();
    }

    public Task<WorkDto[]> SelectWorksForReminderAsync(Guid recipientId)
    {
        return Task.FromResult(Array.Empty<WorkDto>());
    }

    public Task<WorkDto[]> SelectExpiredWorksAsync(Guid recipientId)
        => SelectWorksAsync(recipientId, WorkStatus.Expired);

    public Task<WorkDto[]> SelectDeletedWorksAsync(Guid recipientId)
        => SelectWorksAsync(recipientId, WorkStatus.Deleted);

    public Task<WorkDto[]> SelectCompletedWorksAsync(Guid recipientId)
        => SelectWorksAsync(recipientId, WorkStatus.Completed);

    private async Task<WorkDto[]> SelectWorksAsync(Guid recipientId, WorkStatus workStatus)
    {
        var workDbos = await workRepository.SelectAsync(recipientId, workStatus);
        return workDbos.Select(workConverter.ToDto).ToArray();
    }
}