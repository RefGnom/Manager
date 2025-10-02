using System;
using System.ComponentModel.DataAnnotations;

namespace Manager.Core.Common.Validation.DataAnnotationsCustom;

public class DateTimeFromUtcNowAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is null or string { Length: 0 })
        {
            return true;
        }

        var dateTime = (DateTime)value;
        return dateTime >= DateTime.UtcNow;
    }

    public override string FormatErrorMessage(string name) =>
        $"The field {name} must be after {DateTime.UtcNow:yyyy-MM-ddTHH:mm:ssZ}";
}