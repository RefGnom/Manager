namespace Manager.Tool;

public class Program
{
    private static void Main(string[] args)
    {
        var argumentsString = string.Join('\n', args);
        Console.WriteLine($"Я выполнился! Параметры: {argumentsString}");
    }
}