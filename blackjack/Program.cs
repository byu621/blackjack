namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        int numDecksInShoe = 6;
        int replay = 10000;
        int replay2 = 5;
        int penetration = 80;

        for (int i = 0; i < replay2; i++)
        {
            Simulation simulation = new(numDecksInShoe, penetration);
            decimal ev = simulation.SimulateStand(replay);
            Console.WriteLine(ev);
        }
    }
}
