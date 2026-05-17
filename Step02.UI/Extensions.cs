namespace Step02.UI;

public static class Extensions
{
    public static void LogOnConsole(this Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Error: {ex.Message}");
        Console.ForegroundColor = ConsoleColor.White;
    }
}
