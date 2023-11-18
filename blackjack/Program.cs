using System.Globalization;
using CsvHelper;

namespace blackjack;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, Blackjack");
        DealerTotalTable dealerTotalTable = new();
        dealerTotalTable.Compute();
        dealerTotalTable.WriteToCsv();

        Try foo = new();
        foo.Compute();
        foo.PrintToConsole();

        // PlayerStandEvTable playerStandEvTable = new(dealerTotalTable);
        // playerStandEvTable.Compute();
        // playerStandEvTable.WriteToCsv();

        // PlayerHitEvTable playerHitEvTable = new(playerStandEvTable);
        // playerHitEvTable.Compute();
        // playerHitEvTable.WriteToCsv();
    }
}