namespace blackjack;

public class Simulation
{
    private readonly int numDecksInShoe;
    private readonly int penetration;

    public Simulation(int numDecksInShoe, int penetration)
    {
        this.numDecksInShoe = numDecksInShoe;
        this.penetration = penetration;
    }

    public decimal SimulateStand(int replay)
    {
        decimal ev = 0;
        for (int i = 0; i < replay; i++)
        {
            ev += Simulate();
        }

        return ev/replay;
    }

    public decimal Simulate()
    {
        Shoe shoe = new Shoe(numDecksInShoe);
        decimal runningEv = 0;
        int count = 0;
        while (shoe.isLive(penetration))
        {
            count++;
            Hand dealer = new();
            (dealer, _) = dealer.Hit(shoe.Pop());
            (dealer, _) = dealer.Hit(shoe.Pop());

            Hand player = new();
            (player, _) = player.Hit(shoe.Pop());
            (player, _) = player.Hit(shoe.Pop());

            if (player.Blackjack && dealer.Blackjack)
            {
                continue;
            }

            if (player.Blackjack)
            {
                runningEv += 1.5m;
                continue;
            }
            
            if (dealer.Blackjack)
            {
                runningEv -= 1;
                continue;
            }

            bool dealerBust = false;
            while (!dealerBust && dealer.Value < 17)
            {
                (dealer, dealerBust) = dealer.Hit(shoe.Pop());
            }

            if (dealerBust || dealer.Value < player.Value) {
                runningEv += 1;
                continue;
            }

            if (dealer.Value == player.Value)
            {
                continue;
            }

            if (dealer.Value > player.Value)
            {
                runningEv -= 1;
                continue;
            }

            throw new ArgumentException();
        }

        return runningEv/count;
    }
}