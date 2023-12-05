namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        int numDecksInShoe = 6;
        int numShoe = 100000;
        int penetration = 80;

        SimulationStand simulationStand = new(numDecksInShoe, penetration);
        (decimal standEv, StandTable standTable) = simulationStand.SimulateStand(numShoe);

        Console.WriteLine(standTable.ToString());
        Console.WriteLine((standEv * 100).ToString("0.00") + "%");
    }
}
