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

    public decimal Simulate(int replay)
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
        int playerWin = 0;
        int playerPush = 0;
        int playerLose = 0;
        while (shoe.isLive(penetration))
        {
            Hand dealer = new();
            (dealer, _) = dealer.Hit(shoe.Pop());
            (dealer, _) = dealer.Hit(shoe.Pop());

            Hand player = new();
            (player, _) = player.Hit(shoe.Pop());
            (player, _) = player.Hit(shoe.Pop());

            if (player.Blackjack && dealer.Blackjack)
            {
                playerPush += 1;
                continue;
            }

            if (player.Blackjack)
            {
                playerWin += 1;
                continue;
            }
            
            if (dealer.Blackjack)
            {
                playerLose += 1;
                continue;
            }

            bool dealerBust = false;
            while (!dealerBust && dealer.Value < 17)
            {
                (dealer, dealerBust) = dealer.Hit(shoe.Pop());
            }

            if (dealerBust || dealer.Value < player.Value) {
                playerWin += 1;
                continue;
            }

            if (dealer.Value == player.Value)
            {
                playerPush += 1;
                continue;
            }

            if (dealer.Value > player.Value)
            {
                playerLose += 1;
                continue;
            }

            throw new ArgumentException();
        }

        decimal winPercentage = (decimal) playerWin / (playerWin + playerLose);
        decimal losePercentage = 1 - winPercentage;
        decimal ev = winPercentage - losePercentage;
        return ev;
    }
}