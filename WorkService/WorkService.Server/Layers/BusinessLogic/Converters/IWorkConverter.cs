using WorkService.Server.Layers.BusinessLogic.Models;
using WorkService.Server.Layers.Repository.Dbos;

namespace WorkService.Server.Layers.BusinessLogic.Converters;

public interface IWorkConverter
{
    WorkDbo ToDbo(WorkDto workDto);
    WorkDto ToDto(WorkDbo workDbo);
}

public class WorkConverter : IWorkConverter
{
    public WorkDbo ToDbo(WorkDto workDto)
    {
        return new WorkDbo
        {
            Id = workDto.Id,
            RecipientId = workDto.RecipientId,
            Title = workDto.Title,
            Description = workDto.Description,
            WorkStatus = workDto.WorkStatus,
            CreatedUtc = workDto.CreatedUtc,
            DeadLineUtc = workDto.DeadLineUtc,
            ReminderIntervals = workDto.ReminderIntervals,
        };
    }

    public WorkDto ToDto(WorkDbo workDbo)
    {
        return new WorkDto(
            workDbo.Id,
            workDbo.RecipientId,
            workDbo.Title,
            workDbo.Description,
            workDbo.WorkStatus,
            workDbo.CreatedUtc,
            workDbo.DeadLineUtc,
            workDbo.ReminderIntervals
        );
    }
}