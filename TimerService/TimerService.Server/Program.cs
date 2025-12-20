using Manager.Core.AppConfiguration;
using Manager.Core.HostApp;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

[assembly: ServerProperties("TIMER_SERVICE_PORT", "manager-timer-service")]

namespace Manager.TimerService.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var managerHostApp = new ManagerHostApp<HostAppConfigurator>(args);
        managerHostApp.Run();
    }
}