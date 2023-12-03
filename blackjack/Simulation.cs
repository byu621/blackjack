namespace blackjack;

public class Simulation
{
    private readonly int numDecksInShoe;
    private readonly int replay;
    private readonly int penetration;

    public Simulation(int numDecksInShoe, int replay, int penetration)
    {
        this.numDecksInShoe = numDecksInShoe;
        this.replay = replay;
        this.penetration = penetration;
    }

    public void Simulate()
    {
        Shoe shoe = new Shoe(numDecksInShoe);
        while (shoe.isLive(penetration))
        {
            Hand player = new();
            (player, _) = player.Hit(shoe.Pop());
            (player, _) = player.Hit(shoe.Pop());
            if (player.Blackjack)
            {
                System.Console.WriteLine("blakcjack");
            }
        }
    }
}