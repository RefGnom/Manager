using System;
using System.Linq;

namespace Manager.RecipientService.Server.Implementation.Factories;

public interface ITimeZoneInfoFactory
{
    TimeZoneInfo CreateByOffset(TimeSpan utcOffset);
}

public class TimeZoneInfoFactory : ITimeZoneInfoFactory
{
    public TimeZoneInfo CreateByOffset(TimeSpan utcOffset) =>
        TimeZoneInfo.GetSystemTimeZones().FirstOrDefault(x => x.BaseUtcOffset == utcOffset) ??
        throw new ArgumentException($"Не смогли определить временную зону для сдвига {utcOffset}");
}