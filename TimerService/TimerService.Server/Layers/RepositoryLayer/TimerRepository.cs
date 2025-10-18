using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manager.Core.Common.DependencyInjection.Attributes;
using Manager.Core.EFCore;
using Manager.TimerService.Server.Layers.DbLayer.Dbos;
using Manager.TimerService.Server.Layers.ServiceLayer.Exceptions;
using Manager.TimerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace Manager.TimerService.Server.Layers.RepositoryLayer;

[Scoped]
public class TimerRepository(
    IDbContextWrapperFactory dbContextWrapperFactory,
    IMapper mapper
) : ITimerRepository
{
    public async Task<TimerDto[]> SelectByUserAsync(Guid userId)
    {
        var dbContext = dbContextWrapperFactory.Create();
        return await dbContext.Set<TimerDbo>()
            .Where(x => x.UserId == userId)
            .Select(x => mapper.Map<TimerDto>(x))
            .ToArrayAsync();
    }

    public async Task CreateAsync(TimerDto timerDto)
    {
        var dbContext = dbContextWrapperFactory.Create();
        var timerDbo = mapper.Map<TimerDto, TimerDbo>(timerDto);
        dbContext.Set<TimerDbo>().Add(timerDbo);
        await dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(TimerDto timerDto)
    {
        var dbContext = dbContextWrapperFactory.Create();
        var existedTimer = await FindAsync(dbContext, timerDto.Id);
        if (existedTimer is null)
        {
            throw new NotFoundException("Timer not found");
        }

        mapper.Map(timerDto, existedTimer);
        await dbContext.SaveChangesAsync();
    }

    public async Task<TimerDto?> FindAsync(Guid userId, string timerName)
    {
        var dbContext = dbContextWrapperFactory.Create();
        var timerDbo = await dbContext.Set<TimerDbo>()
            .Where(x => x.UserId == userId)
            .Where(x => x.Name == timerName)
            .FirstOrDefaultAsync();
        return mapper.Map<TimerDto>(timerDbo);
    }

    private async Task<TimerDbo?> FindAsync(DbContextWrapper dbContext, Guid id)
    {
        return await dbContext.Set<TimerDbo>().FirstOrDefaultAsync(x => x.Id == id);
    }
}