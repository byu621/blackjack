namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        int numDecksInShoe = 6;
        int numShoe = 10000;
        int penetration = 80;

        Simulation simulation = new(numDecksInShoe, penetration);
        decimal ev = simulation.SimulateStand(numShoe);
        Console.WriteLine(ev);
    }
}
