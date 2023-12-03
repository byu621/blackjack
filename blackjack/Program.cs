namespace blackjack;

internal static class Program
{
    private static void Main(string[] args)
    {
        int numDecksInShoe = 6;
        int replay = 1;
        int penetration = 80;

        Simulation simulation = new(numDecksInShoe, replay, penetration);
        simulation.Simulate();

        // Hand hand = new Hand();
        // hand.Hit(new Card(10));
    }
}
