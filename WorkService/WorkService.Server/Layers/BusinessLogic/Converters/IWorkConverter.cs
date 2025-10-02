using Manager.WorkService.Server.Layers.BusinessLogic.Models;
using Manager.WorkService.Server.Layers.Repository.Dbos;

namespace Manager.WorkService.Server.Layers.BusinessLogic.Converters;

public interface IWorkConverter
{
    WorkDbo ToDbo(WorkDto workDto);
    WorkDto ToDto(WorkDbo workDbo);
}

public class WorkConverter : IWorkConverter
{
    public WorkDbo ToDbo(WorkDto workDto) => new()
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

    public WorkDto ToDto(WorkDbo workDbo) => new(
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