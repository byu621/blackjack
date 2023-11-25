namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        PlayerHitRunner.WriteToCsv();
        PlayerHitStandRunner.WriteToCsv();
        PlayerDoubleRunner.WriteToCsv();
    }
}