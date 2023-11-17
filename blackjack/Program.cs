namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Blackjack");
        DealerTotalTable dealerTotalTable = new();
        dealerTotalTable.Compute();
        //table.WriteToCsv();

        PlayerStandEvTable playerStandWinTable = new(dealerTotalTable);
        playerStandWinTable.Compute();
        playerStandWinTable.WriteToCsv();
    }
}