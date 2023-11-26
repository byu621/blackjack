namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        DealerBlackjackRunner.WriteToCsv();
        DealerTotalRunnner.WriteToCsv();
        PlayerStandRunner.WriteToCsv();
        PlayerHitRunner.WriteToCsv();
        PlayerHitStandRunner.WriteToCsv();
        PlayerDoubleRunner.WriteToCsv();
    }
}