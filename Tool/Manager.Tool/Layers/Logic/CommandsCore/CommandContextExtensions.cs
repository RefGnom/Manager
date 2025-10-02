using System;
using Manager.Core.Common.HelperObjects.Result;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public static class CommandContextExtensions
{
    public static Result<DateTime, string>? GetDateTimeOptionValue(
        this CommandContext context,
        CommandOptionInfo optionInfo
    )
    {
        var stringValue = context.GetOptionValue(optionInfo);
        if (stringValue is null)
        {
            return null;
        }

        return DateTime.TryParse(stringValue, out var dateTimeValue)
            ? dateTimeValue
            : $"Can't parse date time \"{stringValue}\"";
    }

    public static Result<TimeSpan, string>? GetTimeSpanOptionValue(
        this CommandContext context,
        CommandOptionInfo optionInfo
    )
    {
        var stringValue = context.GetOptionValue(optionInfo);
        if (stringValue is null)
        {
            return null;
        }

        return TimeSpan.TryParse(stringValue, out var dateTimeValue)
            ? dateTimeValue
            : $"Can't parse time \"{stringValue}\"";
    }
}