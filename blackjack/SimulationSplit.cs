namespace blackjack;

public class SimulationSplit(int numDecksInShoe, int penetration, StandTable standTable)
{
    private readonly HitTable _splitTable = new("split");
    
    public (decimal, HitTable) SimulateSplit(int numShoe)
    {
        decimal ev = 0;
        for (int i = 0; i < numShoe; i++)
        {
            ev += SimulateSplit();
        }

        return (ev/numShoe, _splitTable);
    }

    private decimal SimulateSplit()
    {
        Shoe shoe = new Shoe(numDecksInShoe);
        decimal runningEv = 0;
        int count = 0;
        while (shoe.isLive(penetration))
        {
            count++;
            Hand dealer = shoe.Deal();
            Card dealerUpCard = dealer.FirstCard!;
            Hand player = shoe.Deal();
            
            (bool isBlackjack, decimal blackjackEv) = player.EvaluateBlackjack(dealer);
            if (isBlackjack)
            {
                runningEv += blackjackEv;
                continue;
            }
            
            (Hand playerHit, bool playerBust) = player.Hit(shoe.Pop());
            if (playerBust)
            {
                runningEv += -2m;
                _splitTable.Add(dealerUpCard, player, -2m);
                continue;
            }
            
            if (playerHit.Value == 21)
            {
                (dealer, bool dealerBust) = shoe.DealerHit(dealer);
                decimal ev = playerHit.EvaluateHand(dealer, dealerBust) * 2;
                runningEv += ev;
                _splitTable.Add(dealerUpCard, player, ev);
                continue;
            }
            
            decimal standEv = standTable.Get(dealerUpCard, playerHit) * 2;
            runningEv += standEv;
            _splitTable.Add(dealerUpCard, player, standEv);
        }

        return runningEv/count;
    }
}