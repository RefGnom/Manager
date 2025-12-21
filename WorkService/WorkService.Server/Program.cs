using Manager.Core.AppConfiguration;
using Manager.Core.HostApp;

[assembly: ServerProperties("WORK_SERVICE_PORT", "manager-work-service")]

namespace Manager.WorkService.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var managerHostApp = new ManagerHostApp<HostAppConfigurator>(args);
        managerHostApp.Run();
    }
}