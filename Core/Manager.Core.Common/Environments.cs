namespace Manager.Core.Common;

public static class Environments
{
    public static readonly string Development = Microsoft.Extensions.Hosting.Environments.Development;
    public static readonly string Staging = Microsoft.Extensions.Hosting.Environments.Staging;
    public static readonly string Production = Microsoft.Extensions.Hosting.Environments.Production;
    public static readonly string Testing = "Testing";
}