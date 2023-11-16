namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Blackjack");
        DealerTotalTable dealerTotalTable = new();
        dealerTotalTable.Compute();
        //table.WriteToCsv();

        PlayerStandWinTable playerStandWinTable = new(dealerTotalTable);
        playerStandWinTable.Compute();
        playerStandWinTable.WriteToCsv();
    }
}