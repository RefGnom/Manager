using System;
using Manager.Core.DateTimeProvider;
using WorkService.Server.Layers.Api.Requests;
using WorkService.Server.Layers.Api.Responses;
using WorkService.Server.Layers.BusinessLogic.Models;

namespace WorkService.Server.Layers.Api.Converters;

public interface IWorkConverter
{
    WorkDto ToDto(CreateWorkRequest createWorkRequest, Guid workId);
    WorkDto ToDto(WorkDto workDto, PatchWorkRequest patchWorkRequest);
    GetWorkResponse ToResponse(WorkDto workDto);
}

public class WorkConverter(
    IDateTimeProvider dateTimeProvider
) : IWorkConverter
{
    public WorkDto ToDto(CreateWorkRequest createWorkRequest, Guid workId)
    {
        return new WorkDto(
            workId,
            createWorkRequest.RecipientId,
            createWorkRequest.Title,
            createWorkRequest.Description,
            WorkStatus.Actual,
            dateTimeProvider.UtcNow,
            createWorkRequest.DeadLineUtc,
            createWorkRequest.ReminderIntervals
        );
    }

    public WorkDto ToDto(WorkDto workDto, PatchWorkRequest patchWorkRequest)
    {
        return new WorkDto(
            patchWorkRequest.Id,
            workDto.RecipientId,
            patchWorkRequest.Title ?? workDto.Title,
            patchWorkRequest.Description ?? workDto.Description,
            workDto.WorkStatus,
            workDto.CreatedUtc,
            patchWorkRequest.DeadLineUtc ?? workDto.DeadLineUtc,
            patchWorkRequest.ReminderIntervals ?? workDto.ReminderIntervals
        );
    }

    public GetWorkResponse ToResponse(WorkDto workDto)
    {
        return new GetWorkResponse
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
}