using System;
using System.Linq;

namespace Manager.Core.Common.Reflection;

public static class TypeExtensions
{
    public static bool HasInterface<TInterface>(this Type type) => type.GetInterfaces().Contains(typeof(TInterface));
}