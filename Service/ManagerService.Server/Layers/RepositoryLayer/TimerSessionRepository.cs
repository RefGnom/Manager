using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Manager.Core.DependencyInjection.LifetimeAttributes;
using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.RepositoryLayer;

[Scoped]
public class TimerSessionRepository(
    IMapper mapper,
    ManagerDbContext dbContext
) : ITimerSessionRepository
{
    private readonly ManagerDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateOrUpdateAsync(TimerSessionDto timerSessionDto)
    {
        throw new NotImplementedException();
    }

    public Task<TimerSessionDto[]> SelectByTimer(Guid timerId)
    {
        return _dbContext.TimerSessions
            .Where(x => x.TimerId == timerId)
            .Select(x => _mapper.Map<TimerSessionDto>(x))
            .ToArrayAsync();
    }
}