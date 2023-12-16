namespace blackjack;

public class Simulation(int numDecksInShoe, int penetration, int bettingUnit, BasicStrategy basicStrategy)
{
    public (int,int) Simulate(int numShoe)
    {
        int total = 0;
        int numHands = 0;
        for (int i = 0; i < numShoe; i++)
        {
             (int subTotal, int subNumHands) = Simulate();
             total += subTotal;
             numHands += subNumHands;
        }

        return (total, numHands);
    }
    
    private (int,int) Simulate()
    {
        int total = 0;
        int numHands = 0;
        Shoe shoe = new(numDecksInShoe);
        while (shoe.IsLive(penetration))
        {
            numHands++;
            int bet = bettingUnit;
            Hand dealer = shoe.Deal();
            Hand player = shoe.Deal();

            if (player.Blackjack || dealer.Blackjack)
            {
                total += EvaluateBlackjack(player, dealer);
                continue;
            }

            Action action = player.IsPair
                ? Action.Split
                : basicStrategy.GetAction(new(player.Shape, player.Value, dealer.DealerUpCard.Value));

            Hand playerAfter;
            if (action == Action.Double)
            {
                playerAfter = PlayerDouble(player, shoe);
                bet *= 2;
            } else
            {
                playerAfter = PlayerAction(player, dealer.DealerUpCard, shoe);
            }
            
            if (playerAfter.IsBust)
            {
                total -= bet;
                continue;
            }
            
            Hand dealerAfter = DealerAction(dealer, shoe);
            if (dealerAfter.IsBust)
            {
                total += bet;
                continue;
            }

            int compareTo = playerAfter.Value.CompareTo(dealerAfter.Value);
            total += bet * compareTo;
        }

        return (total, numHands);
    }

    private Hand PlayerAction(Hand player, Card dealerUpCard, Shoe shoe)
    {
        if (player.IsBust) return player;
        if (player.Value == 21) return player;
        
        Action action = player.IsPair
            ? Action.Split
            : basicStrategy.GetAction(new(player.Shape, player.Value, dealerUpCard.Value));

        if (action == Action.Stand) return player;
        if (action == Action.Hit)
        {
            player = player.Hit(shoe.Pop());
            return PlayerAction(player, dealerUpCard, shoe);
        }
        
        return player;
    }

    private Hand PlayerDouble(Hand player, Shoe shoe)
    {
        player = player.Hit(shoe.Pop());
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