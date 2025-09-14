using System;
using System.Linq;
using System.Threading.Tasks;
using Manager.Core.AppConfiguration.DataBase;
using Manager.WorkService.Server.Layers.BusinessLogic.Models;
using Manager.WorkService.Server.Layers.Repository.Dbos;
using Microsoft.EntityFrameworkCore;

namespace Manager.WorkService.Server.Layers.Repository;

public interface IWorkRepository
{
    Task CreateAsync(WorkDbo workDto);
    Task UpdateAsync(WorkDbo workDto);
    Task<WorkDbo?> FindAsync(Guid workId);
    Task<WorkDbo[]> SelectAsync(Guid recipientId);
    Task<WorkDbo[]> SelectAsync(Guid recipientId, WorkStatus expectedStatus);
}

public class WorkRepository(
    IDataContext dataContext
) : IWorkRepository
{
    public Task CreateAsync(WorkDbo workDto)
    {
        return dataContext.InsertAsync(workDto);
    }

    public Task UpdateAsync(WorkDbo workDto)
    {
        return dataContext.UpdateAsync(workDto);
    }

    public Task<WorkDbo?> FindAsync(Guid workId)
    {
        return dataContext.FindAsync<WorkDbo, Guid>(workId);
    }

    public Task<WorkDbo[]> SelectAsync(Guid recipientId)
    {
        return dataContext.ExecuteReadAsync<WorkDbo, WorkDbo[]>(x => x
            .Where(w => w.RecipientId == recipientId)
            .ToArrayAsync()
        );
    }

    public Task<WorkDbo[]> SelectAsync(Guid recipientId, WorkStatus expectedStatus)
    {
        return dataContext.ExecuteReadAsync<WorkDbo, WorkDbo[]>(x => x
            .Where(w => w.RecipientId == recipientId)
            .Where(w => w.WorkStatus == expectedStatus)
            .ToArrayAsync()
        );
    }
}