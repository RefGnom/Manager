namespace Data;

public class TimerEntity
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Name { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? PingTime { get; set; }
}