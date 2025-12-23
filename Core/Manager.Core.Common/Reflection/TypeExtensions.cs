using System;
using System.Linq;
using System.Reflection;

namespace Manager.Core.Common.Reflection;

public static class TypeExtensions
{
    public static bool HasInterface<TInterface>(this Type type) => type.GetInterfaces().Contains(typeof(TInterface));

    public static bool HasAttribute<TAttribute>(this Type type) where TAttribute : Attribute =>
        type.GetCustomAttribute<TAttribute>() is not null;
}