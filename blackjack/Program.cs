namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Blackjack");
        DealerTotalTable dealerTotalTable = new();
        dealerTotalTable.Compute();
        //table.WriteToCsv();

        PlayerStandEvTable playerStandEvTable = new(dealerTotalTable);
        playerStandEvTable.Compute();
        playerStandEvTable.WriteToCsv();

        PlayerHitEvTable playerHitEvTable = new(playerStandEvTable);
        playerHitEvTable.Compute();
        playerHitEvTable.WriteToCsv();
    }
}