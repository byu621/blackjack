namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Blackjack");
        DealerTotalTable table = new();
        table.Compute();
        table.WriteToCsv();
    }
}