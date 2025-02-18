using System;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Manager.Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(Enum value)
    {
        var descriptionAttribute = value.GetAttribute<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? "";
    }

    public static T? GetAttribute<T>(this Enum value)
        where T : Attribute
    {
        var type = value.GetType();
        var memInfo = type.GetMember(value.ToString());
        return memInfo.Single().GetCustomAttribute<T>();
    }
}