namespace Manager.Tool;

public interface IManagerTool
{
    Task RunAsync(string[] arguments);
}