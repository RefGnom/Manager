using System;

namespace Manager.Core.Common;

public static class ManagerApp
{
    public static string FriendlyName => AppDomain.CurrentDomain.FriendlyName;
}