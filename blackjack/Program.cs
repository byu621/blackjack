namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        int numDecksInShoe = 6;
        int numShoe = 10000;
        int penetration = 80;

        SimulationStand simulationStand = new(numDecksInShoe, penetration);
        (decimal standEv, StandTable standTable) = simulationStand.SimulateStand(numShoe);

        Console.WriteLine(standTable.ToString());
        Console.WriteLine((standEv * 100).ToString("0.00") + "%");

        SimulationHit simulationHit = new(numDecksInShoe, penetration, standTable);
        (decimal hitEv, HitTable hitTable) = simulationHit.SimulateHit(numShoe);
        
        Console.WriteLine(hitTable.ToString());
        Console.WriteLine((hitEv * 100).ToString("0.00") + "%");

        SimulationDouble simulationDouble = new(numDecksInShoe, penetration, standTable);
        (decimal doubleEv, HitTable doubleTable) = simulationDouble.SimulateDouble(numShoe);

        Console.WriteLine(doubleTable.ToString());
        Console.WriteLine((doubleEv * 100).ToString("0.00") + "%");
    }
}
