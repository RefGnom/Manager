using System.Linq;
using Manager.Core.AppConfiguration;
using Manager.Core.HostApp;

[assembly: ServerProperties("AUTHENTICATION_SERVICE_PORT", "manager-authentication-service")]

namespace Manager.AuthenticationService.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var extendedArgs = args.Append(HostAppArguments.IsAuth).ToArray();
        var managerHostApp = new ManagerHostApp<HostAppConfigurator>(extendedArgs);
        managerHostApp.Run();
    }
}