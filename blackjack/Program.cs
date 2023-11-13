namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        Table table = new();
        table.Compute();
        table.WriteToCsv();
    }
}