namespace Data;

public class TimerSessionEntity
{
    public Guid Id { get; set; }
    public Guid TimerId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? StopTime { get; set; }
    public long Ticks;
}