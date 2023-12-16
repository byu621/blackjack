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

            if (action == Action.Double)
            {
                total += PlayerDouble(player, dealer, shoe);
            } else if (action == Action.Hit || action == Action.Stand || action == Action.Split)
            {
                total += PlayerAction(player, dealer, shoe);
            } 
        }

        return (total, numHands);
    }

    private int PlayerAction(Hand player, Hand dealer, Shoe shoe)
    {
        while (true)
        {
            if (player.IsBust) break;
            if (player.Value == 21) break;
            if (player.IsPair) break;
            Action action = basicStrategy.GetAction(new(player.Shape, player.Value, dealer.DealerUpCard.Value));
            if (action == Action.Stand) break;
            if (action == Action.Hit || action == Action.Double)
            {
                player = player.Hit(shoe.Pop());
            }
        }

        if (player.IsBust)
        {
            return -bettingUnit;
        }
            
        Hand dealerAfter = DealerAction(dealer, shoe);
        if (dealerAfter.IsBust)
        {
            return bettingUnit;
        }

        int compareTo = player.Value.CompareTo(dealerAfter.Value);
        return bettingUnit * compareTo;
    }

    private int PlayerDouble(Hand player, Hand dealer, Shoe shoe)
    {
        Hand playerAfter = player.Hit(shoe.Pop());
        
        if (playerAfter.IsBust)
        {
            throw new Exception("Player busted after doubling");
        }
            
        Hand dealerAfter = DealerAction(dealer, shoe);
        if (dealerAfter.IsBust)
        {
            return bettingUnit * 2;
        }

        int compareTo = playerAfter.Value.CompareTo(dealerAfter.Value);
        return bettingUnit * 2 * compareTo;
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