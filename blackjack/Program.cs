namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        int numDecksInShoe = 6;
        int numShoe = 100_000;
        int penetration = 80;

        SimulationStand simulationStand = new(numDecksInShoe, penetration);
        (_, StandTable standTable) = simulationStand.SimulateStand(numShoe);
        standTable.WriteToFile();

        SimulationHit simulationHit = new(numDecksInShoe, penetration, standTable);
        (_, HitTable hitTable) = simulationHit.SimulateHit(numShoe);
        hitTable.WriteToFile();
        
        SimulationDouble simulationDouble = new(numDecksInShoe, penetration, standTable);
        (_, HitTable doubleTable) = simulationDouble.SimulateDouble(numShoe);
        doubleTable.WriteToFile();
    }
}
