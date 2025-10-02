using System;
using Manager.Core.Common.Time;
using Manager.WorkService.Server.Layers.Api.Requests;
using Manager.WorkService.Server.Layers.Api.Responses;
using Manager.WorkService.Server.Layers.BusinessLogic.Models;

namespace Manager.WorkService.Server.Layers.Api.Converters;

public interface IWorkApiConverter
{
    WorkDto ToDto(CreateWorkRequest createWorkRequest, Guid workId);
    WorkDto ToDto(WorkDto workDto, PatchWorkRequest patchWorkRequest);
    GetWorkResponse ToResponse(WorkDto workDto);
}

public class WorkApiConverter(
    IDateTimeProvider dateTimeProvider
) : IWorkApiConverter
{
    public WorkDto ToDto(CreateWorkRequest createWorkRequest, Guid workId) => new(
        workId,
        createWorkRequest.RecipientId,
        createWorkRequest.Title,
        createWorkRequest.Description,
        WorkStatus.Actual,
        dateTimeProvider.UtcNow,
        createWorkRequest.DeadLineUtc,
        createWorkRequest.ReminderIntervals
    );

    public WorkDto ToDto(WorkDto workDto, PatchWorkRequest patchWorkRequest) => new(
        patchWorkRequest.Id,
        workDto.RecipientId,
        patchWorkRequest.Title ?? workDto.Title,
        patchWorkRequest.Description ?? workDto.Description,
        workDto.WorkStatus,
        workDto.CreatedUtc,
        patchWorkRequest.DeadLineUtc ?? workDto.DeadLineUtc,
        patchWorkRequest.ReminderIntervals ?? workDto.ReminderIntervals
    );

    public GetWorkResponse ToResponse(WorkDto workDto) => new()
    {
        Id = workDto.Id,
        RecipientId = workDto.RecipientId,
        Title = workDto.Title,
        Description = workDto.Description,
        WorkStatus = workDto.WorkStatus,
        DeadLineUtc = workDto.DeadLineUtc,
        ReminderIntervals = workDto.ReminderIntervals,
    };
}