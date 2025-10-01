using System.ComponentModel;
using Microsoft.OpenApi.Extensions;

namespace Manager.Core.Common.Enum;

public static class EnumExtensions
{
    /// <summary>
    /// Достаёт значение из атрибута Description у енама
    /// </summary>
    public static string GetDescription(this System.Enum value)
    {
        var descriptionAttribute = value.GetAttributeOfType<DescriptionAttribute>();
        return descriptionAttribute?.Description ?? "";
    }
}