namespace Manager.Core.DateTimeProvider;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime GetCurrentDateTime()
    {
        return DateTime.Now;
    }
}