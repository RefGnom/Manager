using System;

namespace Manager.RecipientService.Server.Implementation.Factories;

public interface ITimeZoneInfoFactory
{
    TimeZoneInfo CreateByOffset(TimeSpan utcOffset);
}