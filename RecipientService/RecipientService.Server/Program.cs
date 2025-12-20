using Manager.Core.AppConfiguration;
using Manager.Core.HostApp;

[assembly: ServerProperties("RECIPIENT_SERVICE_PORT", "manager-recipient-service")]

namespace Manager.RecipientService.Server;

public static class Program
{
    public static void Main(string[] args)
    {
        var managerHostApp = new ManagerHostApp<HostAppConfigurator>(args);
        managerHostApp.Run();
    }
}