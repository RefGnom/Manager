using System;
using System.ComponentModel;
using Microsoft.OpenApi.Extensions;

namespace Manager.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum value)
    {
        var descriptionAttribute = value.GetAttributeOfType<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? "";
    }
}