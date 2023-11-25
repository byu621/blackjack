namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        // DealerTotalRunnner.WriteToCsv();
        PlayerHitRunner.Print();
        PlayerHitRunner.WriteToCsv();
        PlayerHitStandRunner.WriteToCsv();
        PlayerDoubleRunner.WriteToCsv();
    }
}