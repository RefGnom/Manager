using System;

namespace Manager.Core.Common.Time;

/// <summary>
///     Старайся всегда использовать этот класс для упрощённого тестирования
/// </summary>
public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
    long UtcTicks { get; }
}

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.Now;
    public DateTime UtcNow => DateTime.UtcNow;
    public long UtcTicks => DateTime.UtcNow.Ticks;
}