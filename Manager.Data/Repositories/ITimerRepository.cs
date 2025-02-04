namespace Data.Repositories;

public interface ITimerRepository: IRepository<TimerEntity>
{
    Task<List<TimerEntity>> GetAllForUserAsync(Guid userId);
    Task PingTimeAsync(Guid id, DateTime stopTime);
}