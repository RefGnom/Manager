namespace Data.Repositories;

public interface ITimerSessionRepository: IRepository<TimerSessionEntity>
{
    Task<List<TimerSessionEntity>> GetForTimersAsync(Guid timerId);
    Task StopSessionAsync(Guid id, DateTime stopTime);
}