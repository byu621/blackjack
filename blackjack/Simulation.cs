namespace blackjack;

public class Simulation(int numDecksInShoe, int penetration, int bettingUnit)
{
    public int Simulate()
    {
        int total = 0;
        Shoe shoe = new(numDecksInShoe);
        while (shoe.isLive(penetration))
        {
            Hand dealer = shoe.Deal();
            Hand player = shoe.Deal();

            if (player.Blackjack || dealer.Blackjack)
            {
                total += EvaluateBlackjack(player, dealer);
            }
        }

        return total;
    }
    
    private int EvaluateBlackjack(Hand player, Hand dealer)
    {
        if (player.Blackjack && dealer.Blackjack) return 0;
        if (player.Blackjack) return bettingUnit * 3 / 2;
        if (dealer.Blackjack) return -1 * bettingUnit;
        throw new Exception("Invalid state");
    }
}