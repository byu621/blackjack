namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        BasicStrategy basicStrategy = new BasicStrategy();
        
        int numDecksInShoe = 6;
        int penetration = 80;
        int bettingUnit = 10;
        Simulation simulation = new(numDecksInShoe, penetration, bettingUnit);
        int profit = simulation.Simulate();

        Console.WriteLine(profit);
    }
}
