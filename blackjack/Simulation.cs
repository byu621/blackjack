namespace blackjack;

public class Simulation(int numDecksInShoe, int penetration, int bettingUnit, BasicStrategy basicStrategy)
{
    public int Simulate()
    {
        int total = 0;
        Shoe shoe = new(numDecksInShoe);
        while (shoe.IsLive(penetration))
        {
            Hand dealer = shoe.Deal();
            Hand player = shoe.Deal();

            if (player.Blackjack || dealer.Blackjack)
            {
                total += EvaluateBlackjack(player, dealer);
                continue;
            }

            Hand playerAfter = PlayerAction(player, dealer.DealerUpCard);
            if (playerAfter.IsBust)
            {
                total -= bettingUnit;
                continue;
            }
            
            Hand dealerAfter = DealerAction(dealer, shoe);
            if (dealerAfter.IsBust)
            {
                total += bettingUnit;
                continue;
            }

            int compareTo = playerAfter.Value.CompareTo(dealerAfter.Value);
            total += bettingUnit * compareTo;
        }

        return total;
    }

    private Hand PlayerAction(Hand player, Card dealerUpCard)
    {
        Action action = player.IsPair
            ? Action.Split
            : basicStrategy.GetAction(new(player.Shape, player.Value, dealerUpCard.Value));
        return player;
    }

    private Hand DealerAction(Hand dealer, Shoe shoe)
    {
        while (!dealer.IsBust && dealer.Value < 17)
        {
            dealer = dealer.Hit(shoe.Pop());
        }

        return dealer;
    }
    
    private int EvaluateBlackjack(Hand player, Hand dealer)
    {
        if (player.Blackjack && dealer.Blackjack) return 0;
        if (player.Blackjack) return bettingUnit * 3 / 2;
        if (dealer.Blackjack) return -1 * bettingUnit;
        throw new Exception("Invalid state");
    }
}