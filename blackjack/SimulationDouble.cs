namespace blackjack;

public class SimulationDouble(int numDecksInShoe, int penetration, StandTable standTable)
{
    private readonly HitTable _doubleTable = new();
    
    public (decimal, HitTable) SimulateDouble(int numShoe)
    {
        decimal ev = 0;
        for (int i = 0; i < numShoe; i++)
        {
            ev += SimulateDouble();
        }

        return (ev/numShoe, _doubleTable);
    }

    private decimal SimulateDouble()
    {
        Shoe shoe = new Shoe(numDecksInShoe);
        decimal runningEv = 0;
        int count = 0;
        while (shoe.isLive(penetration))
        {
            count++;
            Hand dealer = shoe.Deal();
            Card dealerUpCard = dealer.UpCard!;
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
                _doubleTable.Add(dealerUpCard, player, -2m);
                continue;
            }
            
            if (playerHit.Value == 21)
            {
                (dealer, bool dealerBust) = shoe.DealerHit(dealer);
                decimal ev = playerHit.EvaluateHand(dealer, dealerBust) * 2;
                runningEv += ev;
                _doubleTable.Add(dealerUpCard, player, ev);
                continue;
            }
            
            decimal standEv = standTable.Get(dealerUpCard, playerHit) * 2;
            runningEv += standEv;
            _doubleTable.Add(dealerUpCard, player, standEv);
        }

        return runningEv/count;
    }
}