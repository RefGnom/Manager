using System;
using Manager.Core.Results;

namespace Manager.Tool.Layers.Logic.CommandsCore;

public static class CommandContextExtensions
{
    public static Result<DateTime>? GetDateTimeOptionValue(this CommandContext context, CommandOptionInfo optionInfo)
    {
        var stringValue = context.GetOptionValue(optionInfo);
        if (stringValue is null)
        {
            return null;
        }

        return DateTime.TryParse(stringValue, out var dateTimeValue)
            ? dateTimeValue
            : Result.CreateFailure<DateTime>($"Can't parse date time \"{stringValue}\"");
    }

    public static Result<TimeSpan>? GetTimeSpanOptionValue(this CommandContext context, CommandOptionInfo optionInfo)
    {
        var stringValue = context.GetOptionValue(optionInfo);
        if (stringValue is null)
        {
            return null;
        }

        return TimeSpan.TryParse(stringValue, out var dateTimeValue)
            ? dateTimeValue
            : Result.CreateFailure<TimeSpan>($"Can't parse time \"{stringValue}\"");
    }
}