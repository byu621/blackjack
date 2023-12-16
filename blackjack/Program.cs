namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        BasicStrategy basicStrategy = new BasicStrategy();
        
        int numDecksInShoe = 6;
        int numShoe = 10_000;
        int penetration = 80;
        int bettingUnit = 10;
        Simulation simulation = new(numDecksInShoe, penetration, bettingUnit, basicStrategy);
        (int profit, int numHand) = simulation.Simulate(numShoe);

        Console.WriteLine($"Profit: {profit}");
        Console.WriteLine($"Num hands: {numHand}");
        Console.WriteLine($"Profit per hand: ${(double)profit / numHand:0.00}");
    }
}
