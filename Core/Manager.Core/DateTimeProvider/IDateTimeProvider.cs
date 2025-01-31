using System;

namespace Manager.Core.DateTimeProvider;

public interface IDateTimeProvider
{
    DateTime Now { get; }
    DateTime UtcNow { get; }
}