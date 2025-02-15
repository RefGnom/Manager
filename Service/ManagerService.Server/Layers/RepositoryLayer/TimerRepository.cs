using AutoMapper;
using ManagerService.Server.Layers.DbLayer;
using ManagerService.Server.Layers.DbLayer.Dbos;
using ManagerService.Server.ServiceModels;
using Microsoft.EntityFrameworkCore;

namespace ManagerService.Server.Layers.RepositoryLayer;

public class TimerRepository(
    ManagerDbContext dbContext,
    IMapper mapper,
    ITimerSessionRepository sessionRepository
) : ITimerRepository
{
    private readonly ManagerDbContext _dbContext = dbContext;
    private readonly IMapper _mapper = mapper;

    public async Task CreateOrUpdateAsync(TimerDto timerDto)
    {
        var timerDbo = _mapper.Map<TimerDto, TimerDbo>(timerDto);

        var existedTimer = await FindAsync(timerDto.UserId, timerDto.Name);
        if (existedTimer is null)
        {
            _dbContext.Timers.Add(timerDbo);
        }
        else
        {
            _dbContext.Timers.Update(timerDbo);
        }

        await _dbContext.SaveChangesAsync();
    }

    public async Task<TimerDto[]> SelectByUserAsync(Guid userId)
    {
        var timers = _dbContext.Timers
            .Where(x => x.UserId == userId)
            .Select(x => mapper.Map<TimerDto>(x));
        return await timers
            .ToArrayAsync();
    }

    public async Task<TimerDto?> FindAsync(Guid userId, string timerName)
    {
        var timerDbo = await _dbContext.Timers
            .Where(x => x.UserId == userId)
            .Where(x => x.Name == timerName)
            .FirstOrDefaultAsync();
        return mapper.Map<TimerDto>(timerDbo);
    }
}